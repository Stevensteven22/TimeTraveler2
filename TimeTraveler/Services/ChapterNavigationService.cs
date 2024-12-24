using System;
using System.Collections.Generic;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;

namespace TimeTraveler.Services;

public class ChapterNavigationService : IChapterNavigationService
{
    public void NavigateTo(ViewType view, object parameter = null)
    {
        ViewModelBase viewModel = view switch
        {
            ViewType.BackgroundView => ServiceLocator.Current.BackgroundViewModel,
            ViewType.GameView => ServiceLocator.Current.GameViewModel,
            ViewType.ResultView => ServiceLocator.Current.ResultViewModel,
            ViewType.ReturnView => ServiceLocator.Current.ReturnViewModel,
            
            ViewType.BackgroundThreeView => ServiceLocator.Current.BackgroundThreeViewModel,
            ViewType.GameThreeView => ServiceLocator.Current.GameThreeViewModel,
            ViewType.ResultThreeView => ServiceLocator.Current.ResultThreeViewModel,
            ViewType.ReturnThreeView => ServiceLocator.Current.ReturnThreeViewModel,
            ViewType.BackgroundTwoView =>ServiceLocator.Current.BackgroundTwoViewModel,
            ViewType.GameTwoView =>ServiceLocator.Current.GameTwoViewModel,
            ViewType.ResultTwoView =>ServiceLocator.Current.ResultTwoViewModel,

            ViewType.BackgroundFourView => ServiceLocator.Current.BackgroundFourViewModel,
            ViewType.GameFourView => ServiceLocator.Current.GameFourViewModel,
            ViewType.ResultFourView => ServiceLocator.Current.ResultFourViewModel,
            //在这里扩展章节：继续添加要导航的页面
            _ => throw new Exception("未知的视图。"),
        };

        if (parameter is not null)
        {
            viewModel.SetParameter(parameter);
        }
        
        if (viewModel is GameThreeViewModel gameThreeViewModel)
        {
            gameThreeViewModel.RestartCommand.Execute(null);

        }

        ServiceLocator.Current.MainViewModel.Content = viewModel;
    }

    private Queue<ChapterViewModel> _menuItemsQueue = new Queue<ChapterViewModel>();

    private Stack<ChapterViewModel> _menuItemsStack = new Stack<ChapterViewModel>();

    public void InitializeMenuItemsQueueToNavigable(IEnumerable<ChapterViewModel> menuItems)
    {
        _menuItemsQueue.Clear();
        _menuItemsStack.Clear();
        foreach (var item in menuItems)
        {
            _menuItemsQueue.Enqueue(item);
            item.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsOK")
                {
                    var chapter = sender as ChapterViewModel;
                    if (chapter != null)
                    {
                        if (chapter.IsOK)
                        {
                            JudgeMenuItemsIsEnabled();
                        }
                    }
                }
            };
        }
        JudgeMenuItemsIsEnabled();
    }

    private void JudgeMenuItemsIsEnabled()
    {
        if (_menuItemsStack.Count > 0 && _menuItemsStack.Peek().IsOK)
        {
            _menuItemsStack.Peek().IsEnabled = false;
        }
        if (_menuItemsQueue.Count > 0 && _menuItemsQueue.Peek().Id == 1)
        {
            var item = _menuItemsQueue.Dequeue();
            item.IsEnabled = true;
            _menuItemsStack.Push(item);
        }
        else if (_menuItemsQueue.Count > 0 && _menuItemsStack.Peek().IsOK)
        {
            var item = _menuItemsQueue.Dequeue();
            item.IsEnabled = true;
            _menuItemsStack.Push(item);
        }
    }
}
