using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Data;
using RomajiConverter.App.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Avalonia.Input;

namespace RomajiConverter.App.Views;

public partial class OutputView : UserControl
{
    public OutputView()
    {
        InitializeComponent();

        SpaceCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
        NewLineCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
        RomajiCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
        HiraganaCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
        JPCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
        KanjiHiraganaCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
        CHCheckBox.IsCheckedChanged += ThirdCheckBox_OnIsCheckedChanged;
    }

    /// <summary>
    /// 显示文本
    /// </summary>
    public void RenderText()
    {
        OutputTextBox.Text = GetResultText();
    }

    /// <summary>
    /// 获取结果文本
    /// </summary>
    /// <returns></returns>
    private string GetResultText()
    {
        string GetString(IEnumerable<string> array)
        {
            return string.Join(SpaceCheckBox.IsChecked.ToBool() ? " " : "", array);
        }

        var output = new StringBuilder();
        for (var i = 0; i < App.ConvertedLineList.Count; i++)
        {
            var item = App.ConvertedLineList[i];
            if (RomajiCheckBox.IsChecked.ToBool())
                output.AppendLine(GetString(item.Units.Select(p => p.Romaji)));
            if (HiraganaCheckBox.IsChecked.ToBool())
                output.AppendLine(GetString(item.Units.Select(p => p.Hiragana)));
            if (JPCheckBox.IsChecked.ToBool())
            {
                if (KanjiHiraganaCheckBox.IsChecked.ToBool())
                {
                    var japanese = item.Japanese;
                    var leftParenthesis = App.Config.LeftParenthesis;
                    var rightParenthesis = App.Config.RightParenthesis;

                    var kanjiUnitList = item.Units.Where(p => p.IsKanji);
                    var replacedIndex = 0;
                    foreach (var kanjiUnit in kanjiUnitList)
                    {
                        var kanjiIndex = japanese.IndexOf(kanjiUnit.Japanese, replacedIndex);
                        var hiraganaIndex = kanjiIndex + kanjiUnit.Japanese.Length;
                        japanese = japanese.Insert(hiraganaIndex,
                            $"{leftParenthesis}{kanjiUnit.Hiragana}{rightParenthesis}");
                        replacedIndex = hiraganaIndex;
                    }

                    output.AppendLine(japanese);
                }
                else
                {
                    output.AppendLine(item.Japanese);
                }
            }

            if (CHCheckBox.IsChecked.ToBool() && !string.IsNullOrWhiteSpace(item.Chinese))
                output.AppendLine(item.Chinese);
            if (NewLineCheckBox.IsChecked.ToBool() && i < App.ConvertedLineList.Count - 1)
                output.AppendLine();
        }

        if (App.ConvertedLineList.Any())
            output.Remove(output.Length - Environment.NewLine.Length, Environment.NewLine.Length);
        return output.ToString();
    }

    /// <summary>
    /// 生成文本区的ToggleSwitch通用事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ThirdCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        OutputTextBox.Text = GetResultText();
    }

    /// <summary>
    /// 复制按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void CopyButton_OnTapped(object? sender, TappedEventArgs e)
    {
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        await clipboard.SetTextAsync(OutputTextBox.Text);
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Classes.Add("Load");
    }

    private void Control_OnUnloaded(object? sender, RoutedEventArgs e)
    {
        Classes.Remove("Load");
    }
}