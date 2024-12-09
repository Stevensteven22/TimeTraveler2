using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TimeTraveler.Converters;

public class PercentValueToTranslateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (
            value is double percent
            && percent >= 0d
            && Double.TryParse(parameter as string, out double fixedWidth)
        )
        {
            var translate = Math.Abs(1 - (percent / 100.0)) * fixedWidth;
            return translate;
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
