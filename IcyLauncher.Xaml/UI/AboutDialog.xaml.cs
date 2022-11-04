using IcyLauncher.Helpers;

namespace IcyLauncher.Xaml.UI;

public sealed partial class AboutDialog : ContentDialog
{
    #region General
    readonly string launcherVersion;
    readonly string aPIVersion;

    readonly string product = Computer.Is64 ? "x64" : "x86";
    readonly string licensing = "IcySnex Copyright © 2022\nSome rights reserved";

    readonly string uILayer = $"WinUI {Computer.WinUIVersion.TrimZeros()}";
    readonly string winAppSDK = Computer.WindowsAppSDKVersion.TrimZeros().ToString();

    readonly string os = Computer.OSVersion.TrimZeros().ToString();
    readonly string framework = Computer.RuntimeVersion.TrimZeros().ToString();
    #endregion


    #region Startup
    public AboutDialog(
        Version launcherVersion,
        Version aPIVersion)
    {
        InitializeComponent();

        this.launcherVersion = launcherVersion.ToString();
        this.aPIVersion = aPIVersion.ToString();
    }
    #endregion
}