using CommunityToolkit.Mvvm.Input;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.ViewModels;

public partial class ChapterViewModel : Chapter
{
    [RelayCommand]
    private void Click(ChapterViewModel chapter) { }
}
