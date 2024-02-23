using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RomajiConverter.App.ValueConverters;

public class IsDetailModeToVisibilityValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? true : false;
    }

    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}