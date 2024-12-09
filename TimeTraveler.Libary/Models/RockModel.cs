using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TimeTraveler.Libary.Models;

public partial class RockModel : ObservableObject
{
    public int Id { get; set; }

    public string Name { get; set; }

    [ObservableProperty]
    private bool _isHasElement;
}
