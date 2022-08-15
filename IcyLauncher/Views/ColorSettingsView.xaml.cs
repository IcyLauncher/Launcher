﻿using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class ColorSettingsView : Page
{
    public readonly ColorSettingsViewModel viewModel = App.Provider.GetRequiredService<ColorSettingsViewModel>();

    public ColorSettingsView() =>
        InitializeComponent();
}