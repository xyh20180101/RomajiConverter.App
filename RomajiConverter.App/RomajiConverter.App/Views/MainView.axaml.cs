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

        //�ṩ��ҳ���������
        MainInputView.MainTabControl = MainTabControl;
        MainInputView.MainEditView = MainEditView;
        MainInputView.MainOutputView = MainOutputView;

        MainEditView.MainTabControl = MainTabControl;
        MainEditView.MainOutputView = MainOutputView;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        //��������ɫ
        var faTheme = (FluentAvaloniaTheme)Application.Current.Styles[0];
        faTheme.CustomAccentColor = TopLevel.GetTopLevel(this).PlatformSettings.GetColorValues().AccentColor1;

        //��Ӧ���뷨�����¼�
        TopLevel.GetTopLevel(this).InputPane.StateChanged += InputPaneOnStateChanged;

        //����״̬��
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