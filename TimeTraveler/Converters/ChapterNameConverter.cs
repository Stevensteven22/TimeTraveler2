using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Ursa.Controls;

namespace TimeTraveler.Converters;

public class ChapterNameConverter : IMultiValueConverter
{
    public object? Convert(
        IList<object?> values,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        DualBadge dualBadge = new DualBadge();
        switch (values[0])
        {
            case "第一关":
                dualBadge.Content = "第一幕 「浮世浮生干岩间」";
                break;
            case "第二关":
                dualBadge.Content = "第二幕「辞行久远之躯」";
                break;
            case "第三关":
                dualBadge.Content = "第三幕 「迫近的客星」";
                break;
            case "第四关":
                dualBadge.Content = "第四幕「我们终将重逢」";
                break;
            case "第五关":
                dualBadge.Content = "第五幕「不动鸣神，恒常乐土」";
                break;
            case "第六关":
                dualBadge.Content = "第六幕「无念无想，泡影断灭」";
                break;
            default:
                dualBadge.Content = "未知关卡";
                break;
        }
        
        if (values[1] is bool isCompleted)
        {
            if (isCompleted)
            {
                dualBadge.Background =new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                dualBadge.Background =new SolidColorBrush(Colors.DodgerBlue);
            }
        }

        return dualBadge;
    }
}
