using Avalonia.Controls;

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

        App.Config.IsDetailMode = true;
    }
}