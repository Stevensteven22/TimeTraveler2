using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TimeTraveler.Converters;

public class PercentValueToScaleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double percent && percent >= 0d)
        {
            return percent / 100.0;
        }
        return value;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        return value;
    }
}
