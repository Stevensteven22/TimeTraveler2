using CommunityToolkit.Mvvm.ComponentModel;

namespace TimeTraveler.Libary.Models;

public partial class BatteryModel : ObservableObject
{
    public int Id { get; set; }

    public string Name { get; set; }

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _isClicked;

    public bool IsHasElement { get; set; }
}
