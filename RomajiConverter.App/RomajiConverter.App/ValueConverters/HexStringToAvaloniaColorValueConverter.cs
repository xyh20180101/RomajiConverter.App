using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using RomajiConverter.App.Extensions;

namespace RomajiConverter.App.ValueConverters;

public class HexStringToAvaloniaColorValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value == null) return null;
        return ((string)value).ToAvaloniaColor();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return null;
        return $"#{((Color)value).A:X2}{((Color)value).R:X2}{((Color)value).G:X2}{((Color)value).B:X2}";
    }
}