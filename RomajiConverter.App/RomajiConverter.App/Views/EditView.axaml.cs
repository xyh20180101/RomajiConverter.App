using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Core;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using RomajiConverter.App.ValueConverters;
using System.Resources;
using Avalonia.Interactivity;
using RomajiConverter.App.Controls;
using RomajiConverter.App.Enums;
using RomajiConverter.App.Extensions;
using System.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Avalonia.Controls.Shapes;
using System.Collections.Generic;
using Avalonia.Platform.Storage;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using RomajiConverter.Core.Models;
using RomajiConverter.App.Helpers;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace RomajiConverter.App.Views;

public partial class EditView : UserControl
{
    private static readonly Binding FontSizeBinding = new()
    {
        Source = App.Config,
        Path = nameof(App.Config.EditPanelFontSize),
        Mode = BindingMode.OneWay
    };

    private static readonly Binding SeparatorMarginBinding = new()
    {
        Source = App.Config,
        Path = nameof(App.Config.EditPanelFontSize),
        Mode = BindingMode.OneWay,
        Converter = new FontSizeToMarginValueConverter()
    };

    private static readonly SolidColorBrush SeparatorBackground = new(Color.FromArgb(170, 170, 170, 170));

    public EditView()
    {
        InitializeComponent();

        EditRomajiCheckBox.IsCheckedChanged += EditToggleSwitch_OnIsCheckedChanged;
        EditHiraganaCheckBox.IsCheckedChanged += EditToggleSwitch_OnIsCheckedChanged;
        IsOnlyShowKanjiCheckBox.IsCheckedChanged += EditToggleSwitch_OnIsCheckedChanged;
        BorderVisibilityComboBox.SelectionChanged += BorderVisibilityComboBoxOnSelectionChanged;

        BorderVisibilityComboBox.Items.Add("ȫ����ʾ");
        BorderVisibilityComboBox.Items.Add("�ж������ʱ��ʾ");
        BorderVisibilityComboBox.Items.Add("����");
        BorderVisibilityComboBox.SelectedIndex = 1;
    }

    public TabControl MainTabControl { get; set; }

    public OutputView MainOutputView { get; set; }

    /// <summary>
    /// ToggleSwitch�ؼ�״̬
    /// </summary>
    public (bool Romaji, bool Hiragana, bool IsOnlyShowKanji) ToggleSwitchState => (EditRomajiCheckBox.IsChecked.ToBool(),
        EditHiraganaCheckBox.IsChecked.ToBool(), IsOnlyShowKanjiCheckBox.IsChecked.ToBool());

    /// <summary>
    /// ��Ⱦ�༭���
    /// </summary>
    public void RenderEditPanel()
    {
        foreach (var children in EditPanel.Items)
            if (children.GetType() == typeof(WrapPanel))
            {
                var wrapPanel = (WrapPanel)children;
                wrapPanel.Children.Clear();
            }
            else if (children.GetType() == typeof(Grid))
            {
                var grid = (Grid)children;
                grid.ClearValue(MarginProperty);
                grid.ClearValue(Panel.BackgroundProperty);
            }

        EditPanel.Items.Clear();
        GC.Collect();

        for (var i = 0; i < App.ConvertedLineList.Count; i++)
        {
            var line = App.ConvertedLineList[i];

            var panel = new WrapPanel();

            foreach (var unit in line.Units)
            {
                var group = new EditableLabelGroup(unit)
                {
                    RomajiVisibility = EditRomajiCheckBox.IsChecked.ToBool() ? true : false,
                    BorderVisibilitySetting = (BorderVisibilitySetting)BorderVisibilityComboBox.SelectedIndex
                };
                group.Bind(EditableLabelGroup.MyFontSizeProperty, FontSizeBinding);
                if (EditHiraganaCheckBox.IsChecked.ToBool())
                {
                    if (IsOnlyShowKanjiCheckBox.IsChecked.ToBool() && group.Unit.IsKanji == false)
                        group.HiraganaVisibility = HiraganaVisibility.Hidden;
                    else
                        group.HiraganaVisibility = HiraganaVisibility.Visible;
                }
                else
                {
                    group.HiraganaVisibility = HiraganaVisibility.Collapsed;
                }
                panel.Children.Add(group);
            }

            EditPanel.Items.Add(panel);

            if (line.Units.Length != 0 && i < App.ConvertedLineList.Count - 1)
            {
                var separator = new Grid
                {
                    Height = 1,
                    Background = SeparatorBackground
                };
                separator.Bind(MarginProperty, SeparatorMarginBinding);

                EditPanel.Items.Add(separator);
            }
        }

        EditScrollViewer.ScrollToHome();
    }

    /// <summary>
    /// �༭����ToggleSwitchͨ���¼�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EditToggleSwitch_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var senderName = ((ToggleSwitch)sender).Name;
        foreach (object children in EditPanel.Items)
        {
            WrapPanel wrapPanel;
            if (children.GetType() == typeof(WrapPanel))
                wrapPanel = (WrapPanel)children;
            else
                continue;

            var isLineContainsKanji = wrapPanel.Children.Any(p => ((EditableLabelGroup)p).Unit.IsKanji);

            foreach (EditableLabelGroup editableLabelGroup in wrapPanel.Children)
                switch (senderName)
                {
                    case "EditRomajiCheckBox":
                        editableLabelGroup.RomajiVisibility = EditRomajiCheckBox.IsChecked.ToBool();
                        break;
                    case "EditHiraganaCheckBox":
                        if (EditHiraganaCheckBox.IsChecked.ToBool())
                            if (IsOnlyShowKanjiCheckBox.IsChecked.ToBool() && !editableLabelGroup.Unit.IsKanji)
                                if (isLineContainsKanji)
                                    editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Hidden;
                                else
                                    editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Collapsed;
                            else
                                editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Visible;
                        else
                            editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Collapsed;
                        break;
                    case "IsOnlyShowKanjiCheckBox":
                        if (EditHiraganaCheckBox.IsChecked.ToBool() && editableLabelGroup.Unit.IsKanji == false)
                            if (IsOnlyShowKanjiCheckBox.IsChecked.ToBool())
                                if (isLineContainsKanji)
                                    editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Hidden;
                                else
                                    editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Collapsed;
                            else
                                editableLabelGroup.HiraganaVisibility = HiraganaVisibility.Visible;
                        break;
                }
        }
    }

    /// <summary>
    /// ������ѡ���¼�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BorderVisibilityComboBoxOnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        foreach (object children in EditPanel.Items)
        {
            WrapPanel wrapPanel;
            if (children.GetType() == typeof(WrapPanel))
                wrapPanel = (WrapPanel)children;
            else
                continue;

            foreach (EditableLabelGroup editableLabelGroup in wrapPanel.Children)
                editableLabelGroup.BorderVisibilitySetting =
                    (BorderVisibilitySetting)BorderVisibilityComboBox.SelectedIndex;
        }
    }

    /// <summary>
    /// �����ı���ť�¼�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ConvertTextButton_OnTapped(object? sender, TappedEventArgs e)
    {
        MainTabControl.SelectedIndex = 2;
        MainOutputView.RenderText();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Classes.Add("Load");
    }

    private void Control_OnUnloaded(object? sender, RoutedEventArgs e)
    {
        Classes.Remove("Load");
    }

    private static IReadOnlyList<FilePickerFileType> _jsonFilePickerFileTypes = new List<FilePickerFileType>
    {
        new("*.json")
        {
            Patterns = new[] { "*.json" },
            MimeTypes = new[] { "application/json" }
        }
    };

    private async void ReadButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "�򿪸���ļ�",
            AllowMultiple = false,
            FileTypeFilter = _jsonFilePickerFileTypes
        });

        if (files.Count >= 1)
        {
            Loading.IsVisible = true;
            await using var fs = await files[0].OpenReadAsync();
            using var sr = new StreamReader(fs, Encoding.UTF8);
            var json = await sr.ReadToEndAsync();
            App.ConvertedLineList = JsonConvert.DeserializeObject<List<ConvertedLine>>(json);
            RenderEditPanel();
            Loading.IsVisible = false;
        }
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "�������ļ�",
            DefaultExtension = ".json",
            FileTypeChoices = _jsonFilePickerFileTypes
        });

        if (file is not null)
        {
            await using var fs = await file.OpenWriteAsync();
            await using var sw = new StreamWriter(fs, Encoding.UTF8);
            await sw.WriteAsync(JsonConvert.SerializeObject(App.ConvertedLineList, Formatting.Indented));
        }
    }

    private async void ConvertPictureButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "������ͼƬ",
            DefaultExtension = ".png",
            FileTypeChoices = new List<FilePickerFileType>
            {
                new("*.png")
                {
                    Patterns = new[] { "*.png" },
                    MimeTypes = new[] { "image/png" }
                }
            }
        });

        if (file is not null)
        {
            var renderData = new List<string[][]>();
            foreach (var line in App.ConvertedLineList)
            {
                var renderLine = new List<string[]>();
                foreach (var unit in line.Units)
                {
                    var renderUnit = new List<string>();
                    if (ToggleSwitchState.Romaji)
                        renderUnit.Add(unit.Romaji);
                    if (ToggleSwitchState.Hiragana)
                    {
                        if (ToggleSwitchState.IsOnlyShowKanji)
                            renderUnit.Add(unit.IsKanji ? unit.Hiragana : " ");
                        else
                            renderUnit.Add(unit.Hiragana);
                    }

                    renderUnit.Add(unit.Japanese);
                    renderLine.Add(renderUnit.ToArray());
                }

                renderData.Add(renderLine.ToArray());
            }

            using var image = renderData.ToImage(new GenerateImageHelper.ImageSetting(App.Config));
            await using var fs = await file.OpenWriteAsync();
            image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(fs);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && App.Config.IsOpenExplorerAfterSaveImage)
                Process.Start("explorer.exe", $"/select,\"{file.Path}\"");
        }
    }
}