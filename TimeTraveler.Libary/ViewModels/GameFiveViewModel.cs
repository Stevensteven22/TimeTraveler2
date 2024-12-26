using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Libary.ViewModels;

public partial class GameFiveViewModel : ViewModelBase
{
    readonly IElementalService _elementalService;

    //用于添加加成
    public ObservableCollection<ResultModel> Results { get; } = [];

    public GameFiveViewModel(IElementalService elementalService)
    {
        _elementalService = elementalService;
        CurrentPage = Page1;

        //注册重启事件
        WeakReferenceMessenger.Default.Register<object, string>(this, "OnRestarted", OnRestarted);
    }

    void OnRestarted(object r, object p)
    {
        Reset();
    }

    //显示元素
    public void ShowElement(ElementType elementType1, ElementType? elementType2, Action nextStep)
    {
        if (elementType2 is not null)
        {
            ShowElementViewModel1 = new ShowElementViewModel
            {
                ElementType = elementType1,
                IsShowElementEnabled = true
            };
            ShowElementViewModel2 = new ShowElementViewModel
            {
                ElementType = elementType2.Value,
                IsShowElementEnabled = false
            };
            ShowElementViewModel1.NextStep = () => ShowElementViewModel2.IsShowElementEnabled = true;
            ShowElementViewModel2.NextStep = nextStep;
        }
        else
        {
            ShowElementViewModel1 = new ShowElementViewModel
            {
                ElementType = elementType1,
                IsShowElementEnabled = true,
                NextStep = nextStep
            };
        }
    }

    [ObservableProperty] ShowElementViewModel? _showElementViewModel1;
    [ObservableProperty] ShowElementViewModel? _showElementViewModel2;

    //显示加成
    public void ShowResult(params ElementType[] elementTypes)
    {
        Results.Clear();
        foreach (var elementType in elementTypes)
        {
            switch (elementType)
            {
                case ElementType.FireElemental:
                    Results.Add(new ResultModel
                        {
                            Id = 0,
                            Name = "火元素",
                            ResultElementType = ElementType.FireElemental,
                            IsSelected = false,
                            ImprovedValue2 = 10,
                            Description = "火元素之力: 攻击力+10%。",
                        }
                    );
                    break;
                case ElementType.IceElemental:
                    Results.Add(
                        new ResultModel
                        {
                            Id = 1,
                            Name = "冰元素",
                            ResultElementType = ElementType.IceElemental,
                            IsSelected = false,
                            ImprovedValue2 = 10,
                            Description = "冰元素之力: 生命力+10%。",
                        }
                    );
                    break;
                case ElementType.ThunderElemental:
                    Results.Add(
                        new ResultModel
                        {
                            Id = 4,
                            Name = "雷元素",
                            ResultElementType = ElementType.ThunderElemental,
                            IsSelected = false,
                            ImprovedValue1 = 10,
                            Description = "雷元素之力: 暴击率+10%。",
                        }
                    );
                    break;
                case ElementType.WindElemental:
                    Results.Add(
                        new ResultModel
                        {
                            Id = 2,
                            Name = "风元素",
                            ResultElementType = ElementType.WindElemental,
                            IsSelected = false,
                            ImprovedValue1 = 10,
                            Description = "风元素之力: 闪避率+10%。",
                        }
                    );
                    break;
                case ElementType.RockElemental:
                    Results.Add(
                        new ResultModel
                        {
                            Id = 3,
                            Name = "岩元素",
                            ResultElementType = ElementType.RockElemental,
                            IsSelected = false,
                            ImprovedValue2 = 10,
                            Description = "岩元素之力: 防御力+10%。",
                        }
                    );
                    break;
                case ElementType.NoneElemental:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        _elementalService.InsertOrUpdateElementalAsync(Results);
        WeakReferenceMessenger.Default.Send<object, string>(Results, "ShowFiveResult");
    }

    void Reset()
    {
        CurrentPage = Page1;
        Page2.SelectedItem = null;
        Page3.SelectedItem = null;
        Page6.SelectedItem1 = null;
        Page6.SelectedItem2 = null;
        Page7.SelectedItem = null;
        Page8.SelectedItem = null;
        Page9.SelectedItem1 = null;
        Page9.SelectedItem2 = null;
    }

    [ObservableProperty] IGameFivePage? _currentPage;

    internal GameFivePage1ViewModel Page1 => _page1 ??= new GameFivePage1ViewModel(this);
    GameFivePage1ViewModel? _page1;

    internal GameFivePage2ViewModel Page2 => _page2 ??= new GameFivePage2ViewModel(this);
    GameFivePage2ViewModel? _page2;

    internal GameFivePage3ViewModel Page3 => _page3 ??= new GameFivePage3ViewModel(this);
    GameFivePage3ViewModel? _page3;

    internal GameFivePage4ViewModel Page4 => _page4 ??= new GameFivePage4ViewModel(this);
    GameFivePage4ViewModel? _page4;

    internal GameFivePage5ViewModel Page5 => _page5 ??= new GameFivePage5ViewModel(this);
    GameFivePage5ViewModel? _page5;

    internal GameFivePage6ViewModel Page6 => _page6 ??= new GameFivePage6ViewModel(this);
    GameFivePage6ViewModel? _page6;

    internal GameFivePage7ViewModel Page7 => _page7 ??= new GameFivePage7ViewModel(this);
    GameFivePage7ViewModel? _page7;

    internal GameFivePage8ViewModel Page8 => _page8 ??= new GameFivePage8ViewModel(this);
    GameFivePage8ViewModel? _page8;

    internal GameFivePage9ViewModel Page9 => _page9 ??= new GameFivePage9ViewModel(this);
    GameFivePage9ViewModel? _page9;

    internal GameFivePage10ViewModel Page10 => _page10 ??= new GameFivePage10ViewModel(this);
    GameFivePage10ViewModel? _page10;

    internal GameFivePage11ViewModel Page11 => _page11 ??= new GameFivePage11ViewModel(this);
    GameFivePage11ViewModel? _page11;

    internal GameFivePage12ViewModel Page12 => _page12 ??= new GameFivePage12ViewModel(this);
    GameFivePage12ViewModel? _page12;

    internal GameFivePage13ViewModel Page13 => _page13 ??= new GameFivePage13ViewModel(this);
    GameFivePage13ViewModel? _page13;

    public void SwitchPage(string pageName)
    {
        CurrentPage = pageName switch
        {
            nameof(Page1) => Page1,
            nameof(Page2) => Page2,
            nameof(Page3) => Page3,
            nameof(Page4) => Page4,
            nameof(Page5) => Page5,
            nameof(Page6) => Page6,
            nameof(Page7) => Page7,
            nameof(Page8) => Page8,
            nameof(Page9) => Page9,
            nameof(Page10) => Page10,
            nameof(Page11) => Page11,
            nameof(Page12) => Page12,
            nameof(Page13) => Page13,
            _ => throw new ArgumentOutOfRangeException(nameof(pageName))
        };
    }
}

public interface IGameFivePage;

public partial class GameFivePage1ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;
    [RelayCommand]
    void A()
    {
        GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page2));
    }

    [RelayCommand]
    void B()
    {
        GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page3));
    }
}

public partial class GameFivePage2ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    public ObservableCollection<GamePuzzleOption> Options { get; } =
    [
        new() { Text = "A. 木生火，火生土，土生金，金生水，水生木", IsCorrect = true },
        new() { Text = "B. 木生水，水生火，火生土，土生金，金生木" },
        new() { Text = "C. 木生土，土生金，金生水，水生火，火生木" },
        new() { Text = "D. 木生金，金生火，火生水，水生土，土生木" },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem;

    [RelayCommand]
    void Crack()
    {
        if (SelectedItem?.IsCorrect == true)
        {
            GameFiveViewModel.ShowElement(ElementType.FireElemental, null, () =>
            {
                GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page4));
            });
        }
        else
        {
            GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page1));
        }
    }
}

public partial class GameFivePage3ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    public ObservableCollection<GamePuzzleOption> Options { get; } =
    [
        new()
        {
            Text = "雷电将军",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/雷电将军.png")),
        },

        new()
        {
            Text = "辛焱",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/辛焱.png")),
            IsCorrect = true
        }
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem;

    [RelayCommand]
    void Crack()
    {
        if (SelectedItem?.IsCorrect == true)
        {
            GameFiveViewModel.ShowElement(ElementType.IceElemental, null, () =>
            {
                GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page5));
            });
        }
        else
        {
            GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page1));
        }
    }
}

public partial class GameFivePage4ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    [RelayCommand]
    void A()
    {
        GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page6));
    }

    [RelayCommand]
    void B()
    {
        GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page7));
    }
}

public partial class GameFivePage5ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    [RelayCommand]
    void A()
    {
        GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page8));
    }

    [RelayCommand]
    void B()
    {
        GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page9));
    }
}

public partial class GameFivePage6ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    public ObservableCollection<GamePuzzleOption> Options1 { get; } =
    [
        new()
        {
            Text = "A",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_A.jpg")),
        },

        new()
        {
            Text = "B",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_B.jpg")),
        },
        new()
        {
            Text = "C",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_C.jpg")),
            IsCorrect = true
        },
        new()
        {
            Text = "D",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_D.jpg")),
        },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem1;

    public ObservableCollection<GamePuzzleOption> Options2 { get; } =
    [
        new()
        {
            Text = "A. 空气中的水分与阳光反应",
        },

        new()
        {
            Text = "B. 云与云之间的摩擦产生电荷",
            IsCorrect = true
        },
        new()
        {
            Text = "C. 火山爆发产生的电荷",
        },
        new()
        {
            Text = "D. 地球磁场与太阳风相互作用",
        },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem2;

    [RelayCommand]
    void Crack()
    {
        if (SelectedItem1?.IsCorrect == true && SelectedItem2?.IsCorrect == true)
        {
            GameFiveViewModel.ShowElement(ElementType.WindElemental, ElementType.ThunderElemental, () =>
            {
                GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page10));
            });
        }
        else
        {
            GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page4));
        }
    }
}

public partial class GameFivePage7ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    public ObservableCollection<GamePuzzleOption> Options { get; } =
    [
        new() { Text = "A. 山脉的走势", IsCorrect = true },
        new() { Text = "B. 江河的流向" },
        new() { Text = "C. 地形的凹凸变化" },
        new() { Text = "D. 房屋的结构布局\r\n" },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem;

    [RelayCommand]
    void Crack()
    {
        if (SelectedItem?.IsCorrect == true)
        {
            GameFiveViewModel.ShowElement(ElementType.RockElemental, null, () =>
            {
                GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page11));
            });
        }
        else
        {
            GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page4));
        }
    }
}

public partial class GameFivePage8ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    public ObservableCollection<GamePuzzleOption> Options { get; } =
    [
        new()
        {
            Text = "A",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_A.jpg")),
        },

        new()
        {
            Text = "B",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_B.jpg")),
        },
        new()
        {
            Text = "C",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_C.jpg")),
            IsCorrect = true
        },
        new()
        {
            Text = "D",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_D.jpg")),
        },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem;

    [RelayCommand]
    void Crack()
    {
        if (SelectedItem?.IsCorrect == true)
        {
            GameFiveViewModel.ShowElement(ElementType.WindElemental, null, () =>
            {
                GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page12));
            });
        }
        else
        {
            GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page5));
        }
    }
}

public partial class GameFivePage9ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    public ObservableCollection<GamePuzzleOption> Options1 { get; } =
    [
        new()
        {
            Text = "A",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_A.jpg")),
        },

        new()
        {
            Text = "B",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_B.jpg")),
        },
        new()
        {
            Text = "C",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_C.jpg")),
            IsCorrect = true
        },
        new()
        {
            Text = "D",
            Icon = ImageHelper.LoadFromResource(new Uri("avares://TimeTraveler/Assets/5_D.jpg")),
        },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem1;

    public ObservableCollection<GamePuzzleOption> Options2 { get; } =
    [
        new()
        {
            Text = "A. 空气中的水分与阳光反应",
        },

        new()
        {
            Text = "B. 云与云之间的摩擦产生电荷",
            IsCorrect = true
        },
        new()
        {
            Text = "C. 火山爆发产生的电荷",
        },
        new()
        {
            Text = "D. 地球磁场与太阳风相互作用",
        },
    ];

    [ObservableProperty]
    GamePuzzleOption? _selectedItem2;

    [RelayCommand]
    void Crack()
    {
        if (SelectedItem1?.IsCorrect == true && SelectedItem2?.IsCorrect == true)
        {
            GameFiveViewModel.ShowElement(ElementType.WindElemental, ElementType.ThunderElemental, () =>
            {
                GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page13));
            });
        }
        else
        {
            GameFiveViewModel.SwitchPage(nameof(GameFiveViewModel.Page5));
        }
    }
}

public partial class GameFivePage10ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    [RelayCommand]
    void Complete()
    {
        GameFiveViewModel.ShowResult(ElementType.FireElemental, ElementType.WindElemental, ElementType.ThunderElemental);
    }
}

public partial class GameFivePage11ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    [RelayCommand]
    void Complete()
    {
        GameFiveViewModel.ShowResult(ElementType.FireElemental, ElementType.RockElemental);
    }
}

public partial class GameFivePage12ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    [RelayCommand]
    void Complete()
    {
        GameFiveViewModel.ShowResult(ElementType.IceElemental, ElementType.WindElemental);
    }
}

public partial class GameFivePage13ViewModel(GameFiveViewModel gameFiveViewModel) : ViewModelBase, IGameFivePage
{
    GameFiveViewModel GameFiveViewModel { get; } = gameFiveViewModel;

    [RelayCommand]
    void Complete()
    {
        GameFiveViewModel.ShowResult(ElementType.IceElemental, ElementType.WindElemental, ElementType.ThunderElemental);
    }
}

public partial class ShowElementViewModel : ViewModelBase
{
    [ObservableProperty]
    ElementType _elementType;

    [ObservableProperty]
    bool _isShowElementEnabled;

    [RelayCommand]
    void ClickElement()
    {
        ElementType = ElementType.NoneElemental;
        IsShowElementEnabled = false;
        NextStep?.Invoke();
    }

    public Action? NextStep { get; set; }
}