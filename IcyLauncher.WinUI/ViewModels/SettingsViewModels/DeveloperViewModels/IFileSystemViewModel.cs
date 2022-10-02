namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    string iFileSystem_path = "";

    [ObservableProperty]
    string iFileSystem_destination = "";

    [ObservableProperty]
    bool iFileSystem_overwrite = false;

    [ObservableProperty]
    int iFileSystem_timeout = 60000;

    [ObservableProperty]
    string iFileSystem_content = "Hello World! :)";


    [RelayCommand]
    async Task IFileSystem_FileExists()
    {
        try
        {
            bool result = fileSystem.FileExists(IFileSystem_path);
            await message.ShowAsync("fileSystem.FileExists()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.FileExists()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task IFileSystem_FileWritable()
    {
        try
        {
            bool result = fileSystem.FileWritable(IFileSystem_path);
            await message.ShowAsync("fileSystem.FileWritable()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.FileWritable()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand]
    async Task IFileSystem_CopyFile()
    {
        try
        {
            fileSystem.CopyFile(IFileSystem_path, IFileSystem_destination, IFileSystem_overwrite);
            await message.ShowAsync("fileSystem.CopyFile()", "Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.CopyFile()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
    async Task IFileSystem_CopyFileAAsync(CancellationToken cancellationToken)
    {
            await fileSystem.CopyFileAsync(IFileSystem_path, IFileSystem_destination, IFileSystem_overwrite, cancellationToken);
            await message.ShowAsync("fileSystem.CopyFileAsync()", "Method completed", closeButton: "Ok");
    }


    [RelayCommand]
    async Task IFileSystem_DeleteFile()
    {
        try
        {
            fileSystem.DeleteFile(IFileSystem_path);
            await message.ShowAsync("fileSystem.DeleteFile()", "Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DeleteFile()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
    async Task IFileSystem_DeleteFileAAsync(CancellationToken cancellationToken)
    {
        try
        {
            await fileSystem.DeleteFileAsync(IFileSystem_path, IFileSystem_timeout, cancellationToken);
            await message.ShowAsync("fileSystem.DeleteFileAsync()", "Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DeleteFileAsync()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
    async Task IFileSystem_WaitForFileLockAsync(CancellationToken cancellationToken)
    {
        try
        {
            bool result = await fileSystem.WaitForFileLockAsync(IFileSystem_path, IFileSystem_timeout, cancellationToken);
            await message.ShowAsync("fileSystem.WaitForFileLockAsync()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.WaitForFileLockAsync()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
    async Task IFileSystem_ReadAsTextAsync(CancellationToken cancellationToken)
    {
        try
        {
            string result = await fileSystem.ReadAsTextAsync(IFileSystem_path, cancellationToken);
            await message.ShowAsync("fileSystem.ReadAsTextAsync()", $"Method completed.\nResult: {(result.Length > 200 ? $"{result[..200]}..." : result)}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.ReadAsTextAsync()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
    async Task IFileSystem_SaveAsTextAsync(CancellationToken cancellationToken)
    {
        try
        {
            await fileSystem.SaveAsTextAsync(IFileSystem_path, IFileSystem_content, IFileSystem_overwrite, cancellationToken);
            await message.ShowAsync("fileSystem.SaveAsTextAsync()", "Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.SaveAsTextAsync()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand]
    async Task IFileSystem_DirectoryExists()
    {
        try
        {
            bool result = fileSystem.DirectoryExists(IFileSystem_path);
            await message.ShowAsync("fileSystem.DirectoryExists()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DirectoryExists()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand]
    async Task IFileSystem_DirectoryWritable()
    {
        try
        {
            bool result = fileSystem.DirectoryWritable(IFileSystem_path);
            await message.ShowAsync("fileSystem.DirectoryWritable()", $"Method completed.\nResult: {result}", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.DirectoryWritable()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand]
    async Task IFileSystem_CreateDirectory()
    {
        try
        {
            fileSystem.CreateDirectory(IFileSystem_path);
            await message.ShowAsync("fileSystem.CreateDirectory()", "Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("fileSystem.CreateDirectory()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }
}