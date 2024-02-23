using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace RomajiConverter.App.ValueConverters;

public class FontSizeToMarginValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new Thickness((int)Math.Floor((double)value / 12) * 4);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}