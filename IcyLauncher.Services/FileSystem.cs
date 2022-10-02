using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Services;

public class FileSystem : IFileSystem
{
    readonly ILogger<FileSystem> logger;

    /// <summary>
    /// Service to communicate with the local file system
    /// </summary>
    public FileSystem(
        ILogger<FileSystem> logger)
    {
        this.logger = logger;

        logger.Log("Registered file system");
    }


    /// <summary>
    /// Checks wether a file exists
    /// </summary>
    /// <param name="path">The path to the file to check</param>
    /// <returns>A boolean wether the file exists</returns>
    public bool FileExists(string path) =>
        File.Exists(path);

    /// <summary>
    /// Checks wether a file is writeable
    /// </summary>
    /// <param name="path">The path to the file to check</param>
    /// <returns>A boolean wether the file is writable</returns>
    public bool FileWritable(
        string path)
    {
        try
        {
            FileStream fs = new(path, FileMode.Open, FileAccess.Write);
            fs.Dispose();
            fs.Close();

            logger.Log($"Checked if file writable: [True-{path}]");
            return true;
        }
        catch (Exception)
        {
            logger.Log($"Checked if file writable: [False-{path}]");
            return false;
        }
    }


    /// <summary>
    /// Copies a file to another destination
    /// </summary>
    /// <param name="path">The path to the file to copy</param>
    /// <param name="destination">The path to the destination the file should be copied</param>
    /// <param name="overwrite">The boolean wether the original file should get overwritten if it exists</param>
    /// <exception cref="Exceptions.FileNotExistsOrLocked">Thown if the file does not exist</exception>
    /// <exception cref="Exceptions.FileExits">Thown if a original file already exists and 'overwrite' is false</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission</exception>
    /// <exception cref="ArgumentNullException">Thrown if path or destination is null</exception>
    /// <exception cref="PathTooLongException">Thrown if the specified path, file name, or both exceeded the system-defined maximum length</exception>
    /// <exception cref="IOException">Thrown if an I/O error has occurres</exception>
    /// <exception cref="NotSupportedException">Thrown if path or destination is in an invalid format</exception>
    [Obsolete("Not recommended: Use asnyc method")]
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

    /// <summary>
    /// Copies a file asynchronously to another destination
    /// </summary>
    /// <param name="path">The path to the file to copy</param>
    /// <param name="destination">The path to the destination the file should be copied</param>
    /// <param name="overwrite">The boolean wether the original file should get overwritten if it exists</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <exception cref="Exceptions.FileNotExistsOrLocked">Thown if the file does not exist</exception>
    /// <exception cref="Exceptions.FileExits">Thown if a original file already exists and 'overwrite' is false</exception>
    /// <exception cref="Exceptions.Cancelled">Thrown if the operation was cancelled by the user</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission</exception>
    /// <exception cref="ArgumentNullException">Thrown if path or destination is null</exception>
    /// <exception cref="PathTooLongException">Thrown if the specified path, file name, or both exceeded the system-defined maximum length</exception>
    /// <exception cref="IOException">Thrown if an I/O error has occurres</exception>
    /// <exception cref="NotSupportedException">Thrown if path or destination is in an invalid format</exception>
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
    /// Deletes a file
    /// </summary>
    /// <param name="path">The path to the file to delete to</param>
    /// <exception cref="Exceptions.FileNotExistsOrLocked">Thrown if the file to delete to does not exist or is not writable</exception>
    /// <exception cref="ArgumentNullException">Thrown if path is null</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the specified path is invalid</exception>
    /// <exception cref="NotSupportedException">Thrown if path is in an invalid format</exception>
    /// <exception cref="PathTooLongException">Thrown if the specified path, file name, or both exceed the system-defined maximum length</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission</exception>
    [Obsolete("Not recommended: Use asnyc method")]
    public void DeleteFile(
        string path)
    {
        if (!FileExists(path) || !FileWritable(path))
            throw Exceptions.FileNotExistsOrLocked;

        File.Delete(path);

        logger.Log($"Deleted file [{path}]");
    }

    /// <summary>
    /// Deletes a file asynchronously
    /// </summary>
    /// <param name="path">The path to the file to delete to</param>
    /// <param name="timeout">The time in ms until the operation times out</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <exception cref="Exceptions.FileNotExistsOrLocked">Thrown if the file to delete to does not exist or is not writable</exception>
    /// <exception cref="Exceptions.Cancelled">Thrown if the operation was cancelled by the user</exception>
    /// <exception cref="Exceptions.Timeout">Thrown if the operation hit the timeout</exception>
    /// <exception cref="ArgumentNullException">Thrown if path is null</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the specified path is invalid</exception>
    /// <exception cref="NotSupportedException">Thrown if path is in an invalid format</exception>
    /// <exception cref="PathTooLongException">Thrown if the specified path, file name, or both exceed the system-defined maximum length</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission</exception>
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


    /// <summary>
    /// Waits for a file lock a asynchronously
    /// </summary>
    /// <param name="path">The path to the file to check</param>
    /// <param name="timeout">The time in ms until the operation times out</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <returns>A boolean wether the system waited for the file lock successfully</returns>
    /// <exception cref="Exceptions.FileNotExistsOrLocked">Thrown if the file to check does not exist</exception>
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

            cycles += 5;
            await Task.Delay(5000, cancellationToken).ConfigureAwait(false);
        }

        logger.Log($"Waited for file lock [{path}]");
        return true;
    }


    /// <summary>
    /// Reads a file as text asynchronously
    /// </summary>
    /// <param name="path">The path to read as text</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <returns>The content as string of the file</returns>
    public Task<string> ReadAsTextAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        logger.Log($"Reading all text asynchronous [{path}]");

        return File.ReadAllTextAsync(path, cancellationToken);
    }

    /// <summary>
    /// Saves a string to a file as text asynchronously
    /// </summary>
    /// <param name="path">The path to the file the content should get written to</param>
    /// <param name="content">The content which will be written to the file</param>
    /// <param name="overwrite">The boolean wether the original file should get overwritten if it exists</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <exception cref="Exceptions.FileExits">Thown if a original file already exists and 'overwrite' is false</exception>
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


    /// <summary>
    /// Checks wether a directory exists
    /// </summary>
    /// <param name="path">The path to the directory to check</param>
    /// <returns>A boolean wether the directory exists</returns>
    public bool DirectoryExists(string path) =>
        Directory.Exists(path);

    /// <summary>
    /// Checks wether a directory is writeable
    /// </summary>
    /// <param name="path">The path to the directory to check</param>
    /// <returns>A boolean wether the directory is writable</returns>
    public bool DirectoryWritable(
        string path)
    {
        try
        {
            FileStream stream = File.Create(Path.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose);
            stream.Close();

            logger.Log($"Checked if directory is writeable [True-{path}]");
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            logger.Log($"Checked if directory is writeable [False-{path}]");
            return false;
        }
        catch (Exception)
        {
            logger.Log($"Failed to check if directory is writeable");
            return false;
        }
    }


    /// <summary>
    /// Creates a new directory
    /// </summary>
    /// <param name="path">The path to the directory to create</param>
    /// <exception cref="Exceptions.DirectoryExists">Thrown if directory already exists</exception>
    /// <exception cref="Exceptions.IOException">Thrown if the directory specified by path is a file</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission</exception>
    /// <exception cref="ArgumentNullException">Thrown if path is null</exception>
    /// <exception cref="PathTooLongException">Thrown if the specified path, file name, or both exceeded the system-defined maximum length</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the container path is invalid</exception>
    /// <exception cref="NotSupportedException">Thrown if the path contains a colon character (:) that is not part of a drive label ("C:\")</exception>
    public void CreateDirectory(
        string path)
    {
        if (DirectoryExists(path))
            throw Exceptions.DirectoryExists;

        Directory.CreateDirectory(path);

        logger.Log($"Created directory [{path}]");
    }
}