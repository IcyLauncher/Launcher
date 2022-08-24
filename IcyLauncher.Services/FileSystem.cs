using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Services;

public class FileSystem : IFileSystem
{
    readonly ILogger<FileSystem> logger;

    public FileSystem(ILogger<FileSystem> logger)
    {
        this.logger = logger;

        this.logger.Log("Registered FileSystem");
    }


    public bool FileExists(string path) =>
        File.Exists(path);

    public bool FileWritable(string path)
    {
        try
        {
            FileStream fs = new(path, FileMode.Open, FileAccess.Write);
            fs.Close();
            fs.Dispose();

            logger.Log($"Checked if file writable: true {path}");
            return true;
        }
        catch (Exception)
        {
            logger.Log($"Checked if file writable: false {path}");
            return false;
        }
    }

    public void CopyFile(string path, string destination, bool overwrite)
    {
        if (!FileExists(path))
            throw Exceptions.FileNotExistsOrLocked;

        if (FileExists(destination) && !overwrite)
            throw Exceptions.FileExits;

        File.Copy(path, destination, overwrite);

        logger.Log($"Successfully copied file {path}");
    }

    public void DeleteFile(string path)
    {
        if (!FileExists(path) || !FileWritable(path))
            throw Exceptions.FileNotExistsOrLocked;

        File.Delete(path);

        logger.Log($"Successfully deleted file {path}");
    }

    public async Task DeleteFileAsync(string path, int timeout = 60000, CancellationToken cancellationToken = default)
    {
        if (!FileExists(path) || !FileWritable(path))
            throw Exceptions.FileNotExistsOrLocked;

        using FileSystemWatcher fw = new(Path.GetDirectoryName(path)!) { EnableRaisingEvents = true };
        bool done = false;
        int cycles = 0;

        fw.Deleted += (sender, e) =>
            done = e.Name == Path.GetFileName(path);

        File.Delete(path);
        logger.Log($"Delete file asynchronous: started {path}");

        while (!done)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.Log($"Delete file asynchronous: cancelled {path}");
                return;
            }

            if (cycles >= timeout / 1000)
            {
                logger.Log($"Delete file asynchronous: timeout {path}");
                throw Exceptions.Timeout;
            }
            timeout += 1;
            await Task.Delay(1000, cancellationToken);
        }

        logger.Log($"Delete file asynchronous: finished {path}");
    }

    public async Task<bool> WaitForFileLock(string path, int timeout = 60000, CancellationToken cancellationToken = default)
    {
        if (!FileExists(path))
            throw Exceptions.FileNotExistsOrLocked;

        int cycles = 0;

        while (!FileWritable(path))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.Log($"Waited for file lock: cancelled  {path}");
                return false;
            }

            if (cycles >= timeout / 1000)
            {
                logger.Log($"Waited for file lock: timeout  {path}");
                throw Exceptions.Timeout;
            }
            timeout += 1;
            await Task.Delay(1000, cancellationToken);
        }

        logger.Log($"Waited for file lock: true {path}");
        return true;
    }


    public Task<string> ReadAsTextAsync(string path, CancellationToken cancellationToken = default)
    {
        logger.Log($"Reading all text asynchronous to {path}");

        return File.ReadAllTextAsync(path, cancellationToken);
    }

    public async Task SaveAsTextAsync(string path, string content, bool overwrite, CancellationToken cancellationToken = default)
    {
        logger.Log($"Saving all text asynchronous to {path}");

        if (!FileExists(path) || overwrite)
            await File.WriteAllTextAsync(path, content, cancellationToken);
        else
            throw Exceptions.FileExits;
    }


    public bool DirectoryExists(string directory) =>
        Directory.Exists(directory);

    public bool DirectoryWritable(string directory)
    {
        try
        {
            using (File.Create(Path.Combine(directory, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { }

            logger.Log($"Checked if directory is writeable: true");
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            logger.Log($"Checked if directory is writeable: false");
            return false;
        }
        catch (Exception)
        {
            logger.Log($"Failed checked if directory is writeable");
            return false;
        }
    }
}