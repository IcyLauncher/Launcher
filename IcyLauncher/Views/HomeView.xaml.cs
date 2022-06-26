﻿using IcyLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace IcyLauncher.Views;

public sealed partial class HomeView : Page
{
    readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();

    public HomeView()
    {
        InitializeComponent();
    }
}