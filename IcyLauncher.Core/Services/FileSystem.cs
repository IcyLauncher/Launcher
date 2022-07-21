using System.IO;
using System.Security;
using System.Security.Permissions;
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