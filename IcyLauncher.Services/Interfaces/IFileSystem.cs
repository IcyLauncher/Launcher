using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Services.Interfaces;

public interface IFileSystem
{
    bool FileExists(string path);

    bool FileWritable(string path);


    /// <summary>
    /// Not recommended: Use Async method
    /// </summary>
    void CopyFile(string path, string destination, bool overwrite);

    Task CopyFileAsync(string path, string destination, bool overwrite, CancellationToken cancellationToken = default);


    /// <summary>
    /// Not recommended: Use Async method
    /// </summary>
    void DeleteFile(string path);

    Task DeleteFileAsync(string path, int timeout = 60000, CancellationToken cancellationToken = default);


    Task<bool> WaitForFileLockAsync(string path, int timeout = 60000, CancellationToken cancellationToken = default);


    Task<string> ReadAsTextAsync(string path, CancellationToken cancellationToken = default);

    Task SaveAsTextAsync(string path, string content, bool overwrite, CancellationToken cancellationToken = default);


    bool DirectoryExists(string directory);

    bool DirectoryWritable(string directory);
}