using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using RomajiConverter.App.Controls;
using RomajiConverter.App.Enums;

namespace RomajiConverter.App.ValueConverters;

public class BorderVisibilitySettingToBorderBrushColorValueConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Any(x => x is UnsetValueType)) return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        var borderVisibilitySetting = (BorderVisibilitySetting)values[0];
        var isEdit = ((EditableLabel)values[1]).IsEdit;
        var replaceTextCount = ((EditableLabel)values[1]).ReplaceText.Count;

        if (isEdit || borderVisibilitySetting == BorderVisibilitySetting.Hidden)
            return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        if (borderVisibilitySetting == BorderVisibilitySetting.Visible || replaceTextCount > 1)
            return new SolidColorBrush(Color.FromArgb(0xAA, 0x99, 0x99, 0x99));
        return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
    }
}