namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    string iFileSystem_path = "";

    [ObservableProperty]
    string iFileSystem_destination = "";

    [ObservableProperty]
    bool iFileSystem_overwrite = false;


    [RelayCommand]
    async Task IFileSystem_FileExists() =>
            await message.ShowAsync("FileExsists - Result", $"Method testing returned: {fileSystem.FileExists(IFileSystem_path)}", closeButton: "Ok");


    [RelayCommand]
    async Task IFileSystem_FileWritable() =>
            await message.ShowAsync("FileWritable - Result", $"Method testing returned: {fileSystem.FileWritable(IFileSystem_path)}", closeButton: "Ok");


    [RelayCommand]
    async Task IFileSystem_CopyFile()
    {
        try
        {
            fileSystem.CopyFile(IFileSystem_path, IFileSystem_destination, IFileSystem_overwrite);
            await message.ShowAsync("CopyFile - Result", "Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("CopyFile - Result", $"Method threw exception: {ex.Format()}", closeButton: "Ok");
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
    async Task IFileSystem_CopyFileAAsync(CancellationToken cancellationToken)
    {
        try
        {
            await fileSystem.CopyFileAsync(IFileSystem_path, IFileSystem_destination, IFileSystem_overwrite, cancellationToken);
            await message.ShowAsync("CopyFile - Result", "Method completed", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("CopyFile - Result", $"Method threw exception: {ex.Format()}", closeButton: "Ok");
        }
    }
}