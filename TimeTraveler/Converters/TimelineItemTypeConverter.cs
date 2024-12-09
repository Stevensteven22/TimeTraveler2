using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Ursa.Controls;

namespace TimeTraveler.Converters;

public class TimelineItemTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isOK)
        {
            return isOK ? TimelineItemType.Success : TimelineItemType.Ongoing;
        }

        return TimelineItemType.Ongoing;
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
