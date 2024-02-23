using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using RomajiConverter.App.Controls;
using RomajiConverter.App.Dialogs;
using RomajiConverter.App.Extensions;
using RomajiConverter.App.Models;
using RomajiConverter.Core.Helpers;

namespace RomajiConverter.App.Views;

public partial class InputView : UserControl
{
    public InputView()
    {
        InitializeComponent();
    }

    public TabControl MainTabControl { get; set; }

    public EditView MainEditView { get; set; }

    public OutputView MainOutputView { get; set; }


    /// <summary>
    /// 转换按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ConvertButton_OnClick(object sender, RoutedEventArgs e)
    {
        Loading.IsVisible = true;

        var inputText = string.Empty;
        var isAutoVariant = false;

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            inputText = InputTextBox.Text;
            isAutoVariant = AutoVariantCheckBox.IsChecked.Value;
        });

        await Task.Run(() =>
        {
            App.ConvertedLineList = RomajiHelper.ToRomaji(inputText, isAutoVariant);
        });

        if (App.Config.IsDetailMode)
        {
            MainEditView.RenderEditPanel();
            MainTabControl.SelectedIndex = 1;
        }
        else
        {
            MainOutputView.RenderText();
            MainTabControl.SelectedItem = 2;
        }
        Loading.IsVisible = false;
    }

    /// <summary>
    /// 设置输入文本
    /// </summary>
    /// <param name="str"></param>
    public void SetTextBoxText(string str)
    {
        InputTextBox.Text = str;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Classes.Add("Load");
    }

    private void Control_OnUnloaded(object? sender, RoutedEventArgs e)
    {
        Classes.Remove("Load");
    }

    /// <summary>
    /// 显示歌词
    /// </summary>
    /// <param name="lrc"></param>
    private void ShowLrc(List<MultilingualLrc> lrc)
    {
        var stringBuilder = new StringBuilder();

        if (lrc.Select(p => p.CLrc).All(p => p.Length == 0))
            // 没有翻译
            foreach (var item in lrc)
                stringBuilder.AppendLine(item.JLrc);
        else
            // 有翻译
            foreach (var item in lrc)
            {
                stringBuilder.AppendLine(item.JLrc);
                stringBuilder.AppendLine(item.CLrc);
            }

        SetTextBoxText(stringBuilder.ToString());
    }

    private async void ImportUrlButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new ImportUrlContentDialog();
        var dialogResult = await dialog.ShowAsync();

        if (dialog.LrcResult.Count != 0) ShowLrc(dialog.LrcResult);
    }
}