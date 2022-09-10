﻿using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Shapes;

namespace IcyLauncher.WinUI.DataTemplates;

public partial class BannerTimeDependentTemplate : ResourceDictionary
{
    public BannerTimeDependentTemplate() =>
        InitializeComponent();


    ImagingUtility imagingUtility = default!;


    void OnImageLoaded(object sender, RoutedEventArgs _)
    {
        if (imagingUtility is null)
            imagingUtility = App.Provider.GetRequiredService<ImagingUtility>();

        Rectangle grid = (Rectangle)sender;
        BannerTimeDependentItem timeDependent = (BannerTimeDependentItem)grid.DataContext;


        imagingUtility.InitializeUIElement(grid, out Compositor? bannerCompositor, out ContainerVisual? banner);

        if (bannerCompositor is null || banner is null)
            return;

        AddImagePart(banner, bannerCompositor, timeDependent.I_3, 1);
        AddImagePart(banner, bannerCompositor, timeDependent.I_6, 2);
        AddImagePart(banner, bannerCompositor, timeDependent.I_9, 3);
        AddImagePart(banner, bannerCompositor, timeDependent.I_12, 4);
        AddImagePart(banner, bannerCompositor, timeDependent.I_15, 5);
        AddImagePart(banner, bannerCompositor, timeDependent.I_21, 7);

        CompositionSurfaceBrush? backgroundBrush = imagingUtility.CreateImageBrush(bannerCompositor, new(timeDependent.I_0), CompositionStretch.UniformToFill);
        SpriteVisual? backgroundVisual = imagingUtility.CreateSpriteVisual(bannerCompositor, new(270f, 66f), backgroundBrush);

        banner.Children.InsertAtBottom(backgroundVisual);
    }

    void AddImagePart(ContainerVisual container, Compositor compositor, string image, int index) =>
        container.Children.InsertAtTop(
            imagingUtility.CreateSpriteVisual(
                compositor,
                new(286.875f - (33.75f * index), 66f),
                imagingUtility.CreateMaskBrush(
                    compositor,
                    imagingUtility.CreateImageBrush(
                        compositor,
                        new(image),
                        CompositionStretch.UniformToFill,
                        1f),
                imagingUtility.CreateGradientBrush(
                    compositor,
                    new(0f, 0f),
                    new(1f, 0f),
                    new[] {
                        (0f, Colors.Transparent),
                        (10f * index / 100, Colors.White)
                    })),
            new((33.75f * index) - 16.875f, 0f, 0f)));
}