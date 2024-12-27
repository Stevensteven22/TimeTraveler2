using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TimeTraveler.Libary.Models;

public partial class GamePuzzleOption:ObservableObject
{
    [ObservableProperty]
    private Bitmap _icon;
    
    [ObservableProperty]
    private string _text;
    
    [ObservableProperty]
    private bool _isSelected;
}