using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Services.Interfaces;

public interface IFileSystem
{
    /// <summary>
    /// Checks wether a file exists
    /// </summary>
    /// <param name="path">The path to the file to check</param>
    /// <returns>A boolean wether the file exists</returns>
    bool FileExists(string path);

    /// <summary>
    /// Checks wether a file is writeable
    /// </summary>
    /// <param name="path">The path to the file to check</param>
    /// <returns>A boolean wether the file is writable</returns>
    bool FileWritable(string path);


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
    void CopyFile(string path, string destination, bool overwrite);

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
    Task CopyFileAsync(string path, string destination, bool overwrite, CancellationToken cancellationToken = default);


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
    void DeleteFile(string path);

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
    Task DeleteFileAsync(string path, int timeout = 60000, CancellationToken cancellationToken = default);


    /// <summary>
    /// Waits for a file lock a asynchronously
    /// </summary>
    /// <param name="path">The path to the file to check</param>
    /// <param name="timeout">The time in ms until the operation times out</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <returns>A boolean wether the system waited for the file lock successfully</returns>
    /// <exception cref="Exceptions.FileNotExistsOrLocked">Thrown if the file to check does not exist</exception>
    Task<bool> WaitForFileLockAsync(string path, int timeout = 60000, CancellationToken cancellationToken = default);


    /// <summary>
    /// Reads a file as text asynchronously
    /// </summary>
    /// <param name="path">The path to read as text</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <returns>The content as string of the file</returns>
    Task<string> ReadAsTextAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a string to a file as text asynchronously
    /// </summary>
    /// <param name="path">The path to the file the content should get written to</param>
    /// <param name="content">The content which will be written to the file</param>
    /// <param name="overwrite">The boolean wether the original file should get overwritten if it exists</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <exception cref="Exceptions.FileExits">Thown if a original file already exists and 'overwrite' is false</exception>
    Task SaveAsTextAsync(string path, string content, bool overwrite, CancellationToken cancellationToken = default);


    /// <summary>
    /// Checks wether a directory exists
    /// </summary>
    /// <param name="path">The path to the directory to check</param>
    /// <returns>A boolean wether the directory exists</returns>
    bool DirectoryExists(string path);

    /// <summary>
    /// Checks wether a directory is writeable
    /// </summary>
    /// <param name="path">The path to the directory to check</param>
    /// <returns>A boolean wether the directory is writable</returns>
    bool DirectoryWritable(string path);

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
    void CreateDirectory(string path);
}