using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RomajiConverter.App.ValueConverters;

public class FontSizeToScaleTextValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)Math.Round((double)value / 14 * 100) + "%";
    }

    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}