using System.Threading;
using System.Threading.Tasks;

namespace IcyLauncher.Core.Interfaces;

public interface IFileSystem
{
    bool FileExists(string path);


    Task<string> ReadAsTextAsync(string path, CancellationToken cancellationToken = default);

    Task SaveAsTextAsync(string path, string content, bool overwrite, CancellationToken cancellationToken = default);


    bool DirectoryExists(string directory);

    bool DirectoryWritable(string directory);
}