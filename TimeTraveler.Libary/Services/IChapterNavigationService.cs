﻿using System.Collections.ObjectModel;
using TimeTraveler.Libary.ViewModels;

namespace TimeTraveler.Libary.Services;

public interface IChapterNavigationService
{
    void NavigateTo(ViewType view, object parameter = null);
    
    void InitializeMenuItemsQueueToNavigable(IEnumerable<ChapterViewModel> menuItems);
}

public static class ChapterNavigationConstant
{
    public static readonly Dictionary<int, List<ViewType>> ChapterViews = new Dictionary<
        int,
        List<ViewType>
    >()
    {
        [1] = new List<ViewType>()
        {
            ViewType.BackgroundView,
            ViewType.GameView,
            ViewType.ResultView,
            ViewType.ReturnView,
        },
        //扩展章节：在这里修改/添加章节的页面
        [2] = new List<ViewType>()
        {
            ViewType.BackgroundView,
            ViewType.GameView,
            ViewType.ResultView,
            ViewType.ReturnView,
        },
        [3] = new List<ViewType>()
        {
            ViewType.BackgroundView,
            ViewType.GameView,
            ViewType.ResultView,
            ViewType.ReturnView,
        },
        [4] = new List<ViewType>()
        {
            ViewType.BackgroundFourView,
            ViewType.GameFourView,
            ViewType.ResultFourView,
            ViewType.ReturnView,
        },
        [5] = new List<ViewType>()
        {
            ViewType.BackgroundView,
            ViewType.GameView,
            ViewType.ResultView,
            ViewType.ReturnView,
        },
        [6] = new List<ViewType>()
        {
            ViewType.BackgroundView,
            ViewType.GameView,
            ViewType.ResultView,
            ViewType.ReturnView,
        },
    };
}

public enum ViewType
{
    BackgroundView,
    BackgroundFourView,
    GameView,
    GameFourView,
    ResultView,
    ResultFourView,
    ReturnView,
}
