using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;

namespace RomajiConverter.App.Controls;

public class SettingsCard : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<SettingsCard, string>(
            nameof(Header), null);

    public static readonly StyledProperty<object> ContentProperty =
        AvaloniaProperty.Register<SettingsCard, object>(
            nameof(Content), null);

    public static readonly StyledProperty<string> GlyphProperty =
        AvaloniaProperty.Register<SettingsCard, string>(
            nameof(Glyph), null);

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

    public BoxShadows BoxShadowEffect = new BoxShadows(new BoxShadow());
}