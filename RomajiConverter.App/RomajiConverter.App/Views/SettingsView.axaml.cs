using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reflection;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using RomajiConverter.App.Extensions;
using FluentAvalonia.UI.Controls;
using System.Resources;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using Avalonia.Media;
using Avalonia.Platform;
using RomajiConverter.App.ValueConverters;
using SkiaSharp;

namespace RomajiConverter.App.Views;

public partial class SettingsView : UserControl
{
    public static List<FontFamily> FontList { get; set; } = new List<FontFamily>
    {
        new FontFamily(new Uri("avares://RomajiConverter.App/Assets/Fonts/Noto Sans SC"),"Noto Sans SC"),
        new FontFamily(new Uri("avares://RomajiConverter.App/Assets/Fonts/Noto Serif SC"),"Noto Serif SC")
    };

    public SettingsView()
    {
        InitializeComponent();

        InitFontFamily();
        VersionTextBlock.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            IsOpenExplorerAfterSaveImageSettingsCard.IsVisible = false;
    }

    /// <summary>
    /// 初始化字体下拉框
    /// </summary>
    private void InitFontFamily()
    {
        foreach (var font in FontList)
        {
            FontFamilyComboBox.Items.Add(font);
        }

        FontFamilyComboBox.Bind(ComboBox.SelectedValueProperty, new Binding
        {
            Mode = BindingMode.TwoWay,
            Source = App.Config,
            Path = nameof(App.Config.FontFamilyName),
            Converter = new StringToFontFamilyValueConverter()
        });
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Classes.Add("Load");
    }

    private void Control_OnUnloaded(object? sender, RoutedEventArgs e)
    {
        Classes.Remove("Load");
    }

    private void FontColorTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        try
        {
            FontColorPicker.Color = FontColorTextBox.Text.ToAvaloniaColor();
        }
        finally
        {
            FontColorTextBox.Text = App.Config.FontColor;
        }
    }

    private void BackgroundColorTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        try
        {
            BackgroundColorPicker.Color = BackgroundColorTextBox.Text.ToAvaloniaColor();
        }
        finally
        {
            BackgroundColorTextBox.Text = App.Config.BackgroundColor;
        }
    }

    private async void ResetButton_OnTapped(object? sender, TappedEventArgs e)
    {
        var contentDialog = new ContentDialog
        {
            Title = "是否恢复默认设置？",
            Content = "现有的设置将不会被保存。",
            CloseButtonText = "取消",
            PrimaryButtonText = "确认",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await contentDialog.ShowAsync();
        if (result == ContentDialogResult.Primary) App.Config.ResetSetting();
    }

    private async void UpdateButton_OnTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            UpdateButton.Opacity = 0;
            UpdateRing.IsActive = true;
            UpdateRing.IsVisible = true;

            var httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(10000);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "RomajiConverter.WinUI Client");
            var httpResponseMessage = await httpClient.GetAsync(new Uri("https://api.github.com/repos/xyh20180101/RomajiConverter.App/releases/latest"), cancellationTokenSource.Token);
            var data = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync(cancellationTokenSource.Token));

            UpdateRing.IsActive = false;
            UpdateRing.IsVisible = false;
            UpdateButton.Opacity = 1;

            var lastVersion = new Version(data["tag_name"].ToString());
            if (lastVersion > Assembly.GetExecutingAssembly().GetName().Version)
            {
                var contentDialog = new ContentDialog
                {
                    Title = "检查更新",
                    Content = "检查到新版本，是否前往下载？",
                    CloseButtonText = "否",
                    PrimaryButtonText = "是",
                    DefaultButton = ContentDialogButton.Primary
                };

                var result = await contentDialog.ShowAsync();

                var launcher = App.ServiceProvider.GetRequiredService<Launcher.Launcher>();
                if (result == ContentDialogResult.Primary)
                    await launcher.LaunchUriAsync(new Uri("https://github.com/xyh20180101/RomajiConverter.App/releases"));
            }
            else
            {
                await new ContentDialog
                {
                    Title = "检查更新",
                    Content = "当前应用已经是最新版本",
                    CloseButtonText = "关闭",
                    DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
            }
        }
        catch (Exception exception)
        {
            await new ContentDialog
            {
                Title = "检查更新",
                Content = "检查更新时出错，请确认你的网络是否正常，或联系开发者",
                CloseButtonText = "关闭",
                DefaultButton = ContentDialogButton.Primary
            }.ShowAsync();
        }
        finally
        {
            UpdateRing.IsActive = false;
            UpdateRing.IsVisible = false;
            UpdateButton.Opacity = 1;
        }
    }
}