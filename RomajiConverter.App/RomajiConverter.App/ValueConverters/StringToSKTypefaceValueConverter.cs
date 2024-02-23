using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace RomajiConverter.App.ValueConverters;

public class StringToFontFamilyValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new FontFamily(new Uri($"avares://RomajiConverter.App/Assets/Fonts/{(string)value}"), (string)value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ((FontFamily)value).Name;
    }
}