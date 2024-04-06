using Avalonia;
using Avalonia.Controls;

namespace RomajiConverter.App.Controls;

public partial class Loading : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Loading, string>(nameof(Text));

    public Loading()
    {
        InitializeComponent();
        DataContext = this;
        Text = "Loading...";
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}