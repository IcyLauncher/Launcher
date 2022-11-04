namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class IFileSystemViewModel : ObservableObject
{
    #region Setup
    readonly IFileSystem fileSystem;
    readonly IMessage message;

    public IFileSystemViewModel(
        IFileSystem fileSystem,
        IMessage message)
    {
        this.fileSystem = fileSystem;
        this.message = message;
    }
    #endregion


    #region Example
    [ObservableProperty]
    string path = "";

    [ObservableProperty]
    string destination = "";

    [ObservableProperty]
    bool overwrite = false;

    [ObservableProperty]
    int timeout = 60000;

    [ObservableProperty]
    string content = "Hello World! :)";
    #endregion


    #region FileExists
    [RelayCommand]
    async Task FileExistsAsync()
    {
        try
        {
            bool result = fileSystem.FileExists(Path);
            await message.ShowAsync("fileSystem.FileExists()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.FileExists()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region FileWritable
    [RelayCommand]
    async Task FileWritableAsync()
    {
        try
        {
            bool result = fileSystem.FileWritable(Path);
            await message.ShowAsync("fileSystem.FileWritable()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.FileWritable()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region CopyFIle
    [RelayCommand]
    async Task CopyFileAsync()
    {
        try
        {
            fileSystem.CopyFile(Path, Destination, Overwrite);
            await message.ShowAsync("fileSystem.CopyFile()", "Method completed");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.CopyFile()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region CopyFileAsync
    [RelayCommand(IncludeCancelCommand = true)]
    async Task CopyFileAAsync(
        CancellationToken cancellationToken)
    {
        await fileSystem.CopyFileAsync(Path, Destination, Overwrite, cancellationToken);
        await message.ShowAsync("fileSystem.CopyFileAsync()", "Method completed");
    }
    #endregion


    #region DeleteFile
    [RelayCommand]
    async Task DeleteFileAsync()
    {
        try
        {
            fileSystem.DeleteFile(Path);
            await message.ShowAsync("fileSystem.DeleteFile()", "Method completed");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DeleteFile()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region DeleteFileAsync
    [RelayCommand(IncludeCancelCommand = true)]
    async Task DeleteFileAAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            await fileSystem.DeleteFileAsync(Path, Timeout, cancellationToken);
            await message.ShowAsync("fileSystem.DeleteFileAsync()", "Method completed");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DeleteFileAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region WaitForFileLockAsync
    [RelayCommand(IncludeCancelCommand = true)]
    async Task WaitForFileLockAAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            bool result = await fileSystem.WaitForFileLockAsync(Path, Timeout, cancellationToken);
            await message.ShowAsync("fileSystem.WaitForFileLockAsync()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.WaitForFileLockAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region ReadAsTextAsync
    [RelayCommand(IncludeCancelCommand = true)]
    async Task ReadAsTextAAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            string result = await fileSystem.ReadAsTextAsync(Path, cancellationToken);
            await message.ShowAsync("fileSystem.ReadAsTextAsync()", $"Method completed.\nResult: {(result.Length > 200 ? $"{result[..200]}..." : result)}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.ReadAsTextAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region SaveAsTextAsync
    [RelayCommand(IncludeCancelCommand = true)]
    async Task SaveAsTextAAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            await fileSystem.SaveAsTextAsync(Path, Content, Overwrite, cancellationToken);
            await message.ShowAsync("fileSystem.SaveAsTextAsync()", "Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.SaveAsTextAsync()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region DirectoryExists
    [RelayCommand]
    async Task DirectoryExistsAsync()
    {
        try
        {
            bool result = fileSystem.DirectoryExists(Path);
            await message.ShowAsync("fileSystem.DirectoryExists()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DirectoryExists()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion

    #region DirectoryWritable
    [RelayCommand]
    async Task DirectoryWritableAsync()
    {
        try
        {
            bool result = fileSystem.DirectoryWritable(Path);
            await message.ShowAsync("fileSystem.DirectoryWritable()", $"Method completed.\nResult: {result}");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DirectoryWritable()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region CreateDirectory
    [RelayCommand]
    async Task CreateDirectoryAsync()
    {
        try
        {
            fileSystem.CreateDirectory(Path);
            await message.ShowAsync("fileSystem.CreateDirectory()", "Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.CreateDirectory()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion
}