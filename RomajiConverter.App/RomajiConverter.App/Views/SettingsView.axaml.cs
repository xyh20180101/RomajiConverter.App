using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using RomajiConverter.App.Extensions;
using RomajiConverter.App.ValueConverters;

namespace RomajiConverter.App.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();

        InitFontFamily();
        VersionTextBlock.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            IsOpenExplorerAfterSaveImageSettingsCard.IsVisible = false;
    }

    public static List<FontFamily> FontList { get; set; } = new()
    {
        new(new Uri("avares://RomajiConverter.App/Assets/Fonts/Noto Sans SC"), "Noto Sans SC"),
        new(new Uri("avares://RomajiConverter.App/Assets/Fonts/Noto Serif SC"), "Noto Serif SC")
    };

    /// <summary>
    /// ��ʼ������������
    /// </summary>
    private void InitFontFamily()
    {
        foreach (var font in FontList) FontFamilyComboBox.Items.Add(font);

        FontFamilyComboBox.Bind(SelectingItemsControl.SelectedValueProperty, new Binding
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
            Title = "�Ƿ�ָ�Ĭ�����ã�",
            Content = "���е����ý����ᱻ���档",
            CloseButtonText = "ȡ��",
            PrimaryButtonText = "ȷ��",
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
            var httpResponseMessage = await httpClient.GetAsync(
                new Uri("https://api.github.com/repos/xyh20180101/RomajiConverter.App/releases/latest"),
                cancellationTokenSource.Token);
            var data = JObject.Parse(
                await httpResponseMessage.Content.ReadAsStringAsync(cancellationTokenSource.Token));

            UpdateRing.IsActive = false;
            UpdateRing.IsVisible = false;
            UpdateButton.Opacity = 1;

            var lastVersion = new Version(data["tag_name"].ToString());
            if (lastVersion > Assembly.GetExecutingAssembly().GetName().Version)
            {
                var contentDialog = new ContentDialog
                {
                    Title = "������",
                    Content = "��鵽�°汾���Ƿ�ǰ�����أ�",
                    CloseButtonText = "��",
                    PrimaryButtonText = "��",
                    DefaultButton = ContentDialogButton.Primary
                };

                var result = await contentDialog.ShowAsync();

                var launcher = App.ServiceProvider.GetRequiredService<Launcher.Launcher>();
                if (result == ContentDialogResult.Primary)
                    await launcher.LaunchUriAsync(
                        new Uri("https://github.com/xyh20180101/RomajiConverter.App/releases"));
            }
            else
            {
                await new ContentDialog
                {
                    Title = "������",
                    Content = "��ǰӦ���Ѿ������°汾",
                    CloseButtonText = "�ر�",
                    DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
            }
        }
        catch (Exception exception)
        {
            await new ContentDialog
            {
                Title = "������",
                Content = "������ʱ������ȷ����������Ƿ�����������ϵ������",
                CloseButtonText = "�ر�",
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

    private async void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
        var launcher = App.ServiceProvider.GetRequiredService<Launcher.Launcher>();
        await launcher.LaunchUriAsync(new Uri("https://github.com/xyh20180101/RomajiConverter.App"));
    }
}