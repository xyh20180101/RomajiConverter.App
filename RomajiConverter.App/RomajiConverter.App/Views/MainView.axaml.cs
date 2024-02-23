using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;
using FluentAvalonia.Styling;

namespace RomajiConverter.App.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        //提供跨页面操作对象
        MainInputView.MainTabControl = MainTabControl;
        MainInputView.MainEditView = MainEditView;
        MainInputView.MainOutputView = MainOutputView;

        MainEditView.MainTabControl = MainTabControl;
        MainEditView.MainOutputView = MainOutputView;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var faTheme = (FluentAvaloniaTheme)App.Current.Styles[0];
        faTheme.CustomAccentColor = TopLevel.GetTopLevel(this).PlatformSettings.GetColorValues().AccentColor1;
    }
}