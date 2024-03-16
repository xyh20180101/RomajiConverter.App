using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;
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
        //设置主题色
        var faTheme = (FluentAvaloniaTheme)Application.Current.Styles[0];
        faTheme.CustomAccentColor = TopLevel.GetTopLevel(this).PlatformSettings.GetColorValues().AccentColor1;

        //响应输入法弹出事件
        TopLevel.GetTopLevel(this).InputPane.StateChanged += InputPaneOnStateChanged;

        //设置状态栏
        var themeVariant =
            TopLevel.GetTopLevel(this).PlatformSettings.GetColorValues().ThemeVariant == PlatformThemeVariant.Light
                ? ThemeVariant.Light
                : ThemeVariant.Dark;
        if (TopLevel.GetTopLevel(this).InsetsManager != null &&
            faTheme.TryGetResource("SolidBackgroundFillColorBase", themeVariant, out var backgroundColor))
            TopLevel.GetTopLevel(this).InsetsManager.SystemBarColor = (Color?)backgroundColor;
    }

    private void InputPaneOnStateChanged(object? sender, InputPaneStateEventArgs e)
    {
        TopLevel.GetTopLevel(this).Padding = new Thickness(0, 0, 0, e.EndRect.Height);
    }
}