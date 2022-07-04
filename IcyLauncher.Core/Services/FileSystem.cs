using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Core.Services;

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
}