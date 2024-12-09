using CommunityToolkit.Mvvm.ComponentModel;
using TimeTraveler.Libary.Definitions;

namespace TimeTraveler.Libary.Models;

public partial class ResultModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _description;

    //属性提升的百分比（提升多少百分比）（闪避率、暴击率），指的是属性加成
    [ObservableProperty]
    private double _improvedValue1;

    //属性提升的数值（提升多少数值）（攻击力、生命力、防御力），指的是属性加成
    [ObservableProperty]
    private double _improvedValue2;

    //属性的实际百分比（不是加成）（闪避率、暴击率）
    [ObservableProperty]
    private double _actualValue1;

    //属性的实际数值（不是加成）（攻击力、生命力、防御力）
    [ObservableProperty]
    private double _actualValue2;

    [ObservableProperty]
    private ElementType resultElementType;

    [ObservableProperty]
    private bool _isSelected;
}
