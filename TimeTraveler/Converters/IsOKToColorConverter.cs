using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TimeTraveler.Converters;

public class IsOKToColorConverter:IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Color red = Color.Parse("#f35538");
        Color green = Color.Parse("#62b26b");

        return value is bool b && b? new SolidColorBrush(green) : new SolidColorBrush(red);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}