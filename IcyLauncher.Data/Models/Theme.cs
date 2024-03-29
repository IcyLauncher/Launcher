﻿using CommunityToolkit.Mvvm.ComponentModel;
using Windows.UI;

namespace IcyLauncher.Data.Models;

public partial class Theme : ObservableObject
{
    public static Theme Dark { get; } = new();

    public static Theme Light { get; } = new()
    {
        Accent = new()
        {
            Primary = Color.FromArgb(255, 0, 138, 255),
            Light = Color.FromArgb(255, 96, 184, 255),
            Dark = Color.FromArgb(255, 24, 83, 196)
        },
        Background = new()
        {
            Solid = Color.FromArgb(255, 223, 223, 223),
            Transparent = Color.FromArgb(180, 255, 255, 255),
            Gradient = Color.FromArgb(255, 223, 223, 223),
            GradientTransparent = Color.FromArgb(0, 223, 223, 223)
        },
        Text = new()
        {
            Primary = Color.FromArgb(255, 0, 0, 0),
            Secondary = Color.FromArgb(196, 0, 0, 0),
            Tertiary = Color.FromArgb(133, 0, 0, 0),
            Disabled = Color.FromArgb(92, 0, 0, 0),
        },
        Control = new()
        {
            Primary = Color.FromArgb(13, 0, 0, 0),
            Outline = Color.FromArgb(26, 0, 0, 0),
            PrimaryDisabled = Color.FromArgb(38, 0, 0, 0),
            OutlineDisabled = Color.FromArgb(51, 0, 0, 0),
            Solid = new()
            {
                Primary = Color.FromArgb(255, 209, 209, 209),
                Outline = Color.FromArgb(255, 199, 199, 199),
                PrimaryDisabled = Color.FromArgb(255, 188, 188, 188),
                OutlineDisabled = Color.FromArgb(255, 176, 176, 176)
            }
        }
    };

    public ThemeAccent Accent { get; set; } = new();

    public ThemeBackground Background { get; set; } = new();

    public ThemeText Text { get; set; } = new();

    public ThemeControl Control { get; set; } = new();
}

public partial class ThemeAccent : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 0, 138, 255);

    [ObservableProperty]
    Color light = Color.FromArgb(255, 96, 184, 255);

    [ObservableProperty]
    Color dark = Color.FromArgb(255, 24, 83, 196);
}

public partial class ThemeBackground : ObservableObject
{
    [ObservableProperty]
    Color solid = Color.FromArgb(255, 32, 32, 32);

    [ObservableProperty]
    Color transparent = Color.FromArgb(150, 32, 32, 32);

    [ObservableProperty]
    Color gradient = Color.FromArgb(255, 0, 0, 0);

    [ObservableProperty]
    Color gradientTransparent = Color.FromArgb(0, 0, 0, 0);
}

public partial class ThemeText : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 255, 255, 255);

    [ObservableProperty]
    Color secondary = Color.FromArgb(196, 255, 255, 255);

    [ObservableProperty]
    Color tertiary = Color.FromArgb(133, 255, 255, 255);

    [ObservableProperty]
    Color disabled = Color.FromArgb(92, 255, 255, 255);
}

public partial class ThemeControl : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(13, 255, 255, 255);

    [ObservableProperty]
    Color outline = Color.FromArgb(26, 255, 255, 255);

    [ObservableProperty]
    Color primaryDisabled = Color.FromArgb(38, 255, 255, 255);

    [ObservableProperty]
    Color outlineDisabled = Color.FromArgb(51, 255, 255, 255);

    public ThemeControlSolid Solid { get; set; } = new();
}
public partial class ThemeControlSolid : ObservableObject
{
    [ObservableProperty]
    Color primary = Color.FromArgb(255, 46, 46, 46);

    [ObservableProperty]
    Color outline = Color.FromArgb(255, 56, 56, 56);

    [ObservableProperty]
    Color primaryDisabled = Color.FromArgb(255, 67, 67, 67);

    [ObservableProperty]
    Color outlineDisabled = Color.FromArgb(255, 79, 79, 79);
}