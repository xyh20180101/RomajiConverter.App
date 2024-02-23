using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RomajiConverter.App.ValueConverters;

public class DoubleToStringValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ((double)value).ToString("F1");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return double.Parse((string)value);
    }
}