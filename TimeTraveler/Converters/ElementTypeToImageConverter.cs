using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Helpers;

namespace TimeTraveler.Converters;

public class ElementTypeToImageConverter: IValueConverter
{
   
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    { ;
        var bitmap = value  switch
        {
            ElementType.FireElemental => ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/火元素.png")),
            ElementType.IceElemental => ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/冰元素.png")),
            ElementType.ThunderElemental => ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/雷元素.png")),
            ElementType.RockElemental => ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/岩元素.png")),
            ElementType.WindElemental => ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/风元素.png")),
            _ => null
        };

        return bitmap;
    }
    
  
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}