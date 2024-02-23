using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace RomajiConverter.App.ValueConverters;

public class IsDetailModeToWidthValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool)value ? new GridLength(1.2, GridUnitType.Star) : new GridLength(0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}