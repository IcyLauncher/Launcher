using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Services;

public class FileSystem : IFileSystem
{
    readonly ILogger<FileSystem> logger;

    public FileSystem(
        ILogger<FileSystem> logger)
    {
        this.logger = logger;

        logger.Log("Registered file system");
    }


    public bool FileExists(string path) =>
        File.Exists(path);

    public bool FileWritable(
        string path)
    {
        try
        {
            FileStream fs = new(path, FileMode.Open, FileAccess.Write);
            fs.Close();
            fs.Dispose();

            logger.Log($"Checked if file writable: [true-{path}]");
            return true;
        }
        catch (Exception)
        {
            logger.Log($"Checked if file writable: [false-{path}]");
            return false;
        }
    }


    /// <summary>
    /// Not recommended: Use Async method
    /// </summary>
    public void CopyFile(
        string path,
        string destination,
        bool overwrite)
    {
        if (!FileExists(path))
            throw Exceptions.FileNotExistsOrLocked;

        if (FileExists(destination) && !overwrite)
            throw Exceptions.FileExits;

        if (!DirectoryExists(Path.GetDirectoryName(destination)!))
            CreateDirectory(Path.GetDirectoryName(destination)!);

        File.Copy(path, destination, overwrite);

        logger.Log($"Copied file [{path}]");
    }

    public async Task CopyFileAsync(
        string path,
        string destination,
        bool overwrite,
        CancellationToken cancellationToken = default)
    {
        if (!FileExists(path))
            throw Exceptions.FileNotExistsOrLocked;

        if (FileExists(destination) && !overwrite)
            throw Exceptions.FileExits;

        if (!DirectoryExists(Path.GetDirectoryName(destination)!))
            CreateDirectory(Path.GetDirectoryName(destination)!);

        FileOptions fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
        int bufferSize = 4096;

        using FileStream sourceStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions);
        using FileStream destinationStream = new(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize, fileOptions);

        await sourceStream.CopyToAsync(destinationStream, bufferSize, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        logger.Log($"Copied file asynchronous [{path}]");
    }


    /// <summary>
    /// Not recommended: Use Async method
    /// </summary>
    public void DeleteFile(
        string path)
    {
        if (!FileExists(path) || !FileWritable(path))
            throw Exceptions.FileNotExistsOrLocked;

        File.Delete(path);

        logger.Log($"Deleted file [{path}]");
    }

    public async Task DeleteFileAsync(
        string path,
        int timeout = 60000,
        CancellationToken cancellationToken = default)
    {
        if (!FileExists(path) || !FileWritable(path))
            throw Exceptions.FileNotExistsOrLocked;

        using FileSystemWatcher fw = new(Path.GetDirectoryName(path)!) { EnableRaisingEvents = true };
        bool done = false;
        int cycles = 0;

        fw.Deleted += (sender, e) =>
            done = e.Name == Path.GetFileName(path);

        File.Delete(path);

        while (!done)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.Log($"Failed to delete file asynchronous [{path}]", Exceptions.Cancelled);
                return;
            }

            if (cycles >= timeout / 1000)
            {
                logger.Log($"Failed to delete file asynchronous [{path}]", Exceptions.Timeout);
                throw Exceptions.Timeout;
            }
            timeout += 1;
            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
        }

        logger.Log($"Deleted file asynchronous [{path}]");
    }


    public async Task<bool> WaitForFileLockAsync(
        string path,
        int timeout = 60000,
        CancellationToken cancellationToken = default)
    {
        if (!FileExists(path))
            throw Exceptions.FileNotExistsOrLocked;

        int cycles = 0;

        while (!FileWritable(path))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.Log($"Failed to wait for file lock [{path}]", Exceptions.Cancelled);
                return false;
            }

            if (cycles >= timeout / 1000)
            {
                logger.Log($"Failed to wait for file lock [{path}]", Exceptions.Timeout);
                return false;
            }

            timeout += 1;
            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
        }

        logger.Log($"Waited for file lock [{path}]");
        return true;
    }


    public Task<string> ReadAsTextAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        logger.Log($"Reading all text asynchronous [{path}]");

        return File.ReadAllTextAsync(path, cancellationToken);
    }

    public async Task SaveAsTextAsync(
        string path,
        string content,
        bool overwrite,
        CancellationToken cancellationToken = default)
    {
        logger.Log($"Saving all text asynchronous [{path}]");

        if (!FileExists(path) || overwrite)
            await File.WriteAllTextAsync(path, content, cancellationToken).ConfigureAwait(false);
        else
            throw Exceptions.FileExits;
    }


    public bool DirectoryExists(string directory) =>
        Directory.Exists(directory);

    public bool DirectoryWritable(
        string directory)
    {
        try
        {
            using (File.Create(Path.Combine(directory, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { }

            logger.Log($"Checked if directory is writeable [true-{directory}]");
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            logger.Log($"Checked if directory is writeable [false-{directory}]");
            return false;
        }
        catch (Exception)
        {
            logger.Log($"Failed to check if directory is writeable");
            return false;
        }
    }


    public void CreateDirectory(
        string directory)
    {
        if (DirectoryExists(directory))
            throw Exceptions.DirectoryExists;

        Directory.CreateDirectory(directory);

        logger.Log($"Created directory [{directory}]");
    }
}