using Avalonia;
using Avalonia.Controls.Primitives;

namespace RomajiConverter.App.Controls;

public class SettingsCard : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<SettingsCard, string>(
            nameof(Header));

    public static readonly StyledProperty<object> ContentProperty =
        AvaloniaProperty.Register<SettingsCard, object>(
            nameof(Content));

    public static readonly StyledProperty<string> GlyphProperty =
        AvaloniaProperty.Register<SettingsCard, string>(
            nameof(Glyph));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public string Glyph
    {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }
}