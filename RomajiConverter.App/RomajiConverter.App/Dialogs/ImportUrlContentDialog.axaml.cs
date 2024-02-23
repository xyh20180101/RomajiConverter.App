using Avalonia.Controls;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using RomajiConverter.App.Helpers.LyricsHelpers;
using RomajiConverter.App.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace RomajiConverter.App.Dialogs;

public partial class ImportUrlContentDialog : ContentDialog, IStyleable
{
    private string _errorText;

    Type IStyleable.StyleKey => typeof(ContentDialog);

    public ImportUrlContentDialog()
    {
        InitializeComponent();

        ErrorText = string.Empty;

        PrimaryButtonClick += OnPrimaryButtonClick;
        Closing += OnClosing;
    }

    public string ErrorText
    {
        get => _errorText;
        set
        {
            if (value == _errorText) return;
            _errorText = value;
            ErrorTextBox.Text = _errorText;
        }
    }

    public List<MultilingualLrc> LrcResult { get; set; } = new();

    private void OnClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
    {
        args.Cancel = args.Result == ContentDialogResult.Primary;
    }

    /*
     * GetLrc������ʱ,���¹رմ���ʱLrcResult��Ϊ��
     * �������:����PrimaryButton��Close�߼�,�ֶ���OnPrimaryButtonClick�����йرմ���
     *
     * ����Hide�޷�ָ��ContentDialogResult,�����������ContentDialogResult����ΪNone
     * ���MainPage��Ҫ�ж�LrcResult�Ƿ�Ϊ��,����������Ⱦ���
     */
    private async void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        var urlRegex = new Regex("http[^\\s]*", RegexOptions.Compiled);

        try
        {
            var url = urlRegex.Match(UrlTextBox.Text ?? string.Empty).Value;

            if (url.Contains("music.163.com"))
            {
                var songId = HttpUtility.ParseQueryString(new Uri(url).Query)["id"];

                LrcResult = await CloudMusicLyricsHelper.GetLrc(songId);
            }
            else if (url.Contains("kugou.com"))
            {
                LrcResult = await KuGouMusicLyricsHelper.GetLrc(url);
            }
            else if (url.Contains("y.qq.com"))
            {
                LrcResult = await QQMusicLyricsHelper.GetLrc(url);
            }
            else
            {
                throw new Exception("������Ч");
            }
        }
        catch (Exception e)
        {
            ErrorText = e.Message;
            return;
        }

        Hide();
    }

    private void TextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        ErrorText = string.Empty;
    }

    private async void UrlTextBox_OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        await Task.Delay(20);
        var b = UrlTextBox.Focus();
    }
}