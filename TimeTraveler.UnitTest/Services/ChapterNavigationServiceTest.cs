using Moq;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;
using Xunit;

namespace TimeTraveler.UnitTest.Services;

public class ChapterNavigationServiceTest
{
    private Mock<IResultVerifyService> _mockResultVerifyService;
    private GameViewModel _gameViewModel;

    [Fact]
    public void NavigateTo_Default()
    {
        var view = ViewType.GameView;
        object parameter = null;
        // 创建 Mock 对象
        _mockResultVerifyService = new Mock<IResultVerifyService>();
        // 使用 Mock 对象创建 GameViewModel 实例
        _gameViewModel = new GameViewModel();
        ViewModelBase viewModel = view switch
        {
            ViewType.BackgroundView => ServiceLocator.Current.BackgroundViewModel,
            ViewType.GameView => _gameViewModel,
            ViewType.ResultView => ServiceLocator.Current.ResultViewModel,
            ViewType.ReturnView => ServiceLocator.Current.ReturnViewModel,
            _ => throw new Exception("未知的视图。"),
        };
        Assert.NotNull(viewModel);
        if (parameter is not null)
        {
            viewModel.SetParameter(parameter);
        }
        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockNavigationService = rootNavigationServiceMock.Object;
        var chapterNavigationServiceMock = new Mock<IChapterNavigationService>();
        var mockChapterNavigationService = chapterNavigationServiceMock.Object;
        var mainViewModel = new MainViewModel(mockChapterNavigationService, mockNavigationService);
        mainViewModel.Content = viewModel;
        Assert.Equal(viewModel, mainViewModel.Content);
    }
    private Queue<ChapterViewModel> _menuItemsQueue = new Queue<ChapterViewModel>();

    private Stack<ChapterViewModel> _menuItemsStack = new Stack<ChapterViewModel>();
    [Fact]
    public void InitializeMenuItemsQueueToNavigable_Default()
    {
        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockNavigationService = rootNavigationServiceMock.Object;
        var chapterNavigationServiceMock = new Mock<IChapterNavigationService>();
        var mockChapterNavigationService = chapterNavigationServiceMock.Object;
        var menuItems= new MainViewModel(mockChapterNavigationService, mockNavigationService).MenuItems;
        _menuItemsStack.Clear();
        _menuItemsQueue.Clear();
        foreach (var item in menuItems)
        {
            _menuItemsQueue.Enqueue(item);
            item.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsOK")
                {
                    var chapter = sender as ChapterViewModel;
                    Assert.NotNull(chapter);
                    if (chapter != null)
                    {
                        Assert.True(chapter.IsOK);
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
            Assert.False(_menuItemsStack.Peek().IsEnabled);
        }
        if (_menuItemsQueue.Count > 0 && _menuItemsQueue.Peek().Id == 1)
        {
            var item = _menuItemsQueue.Dequeue();
            Assert.NotNull(item);
            item.IsEnabled = true;
            _menuItemsStack.Push(item);
            Assert.True(_menuItemsStack.Peek().IsEnabled);
        }
        else if (_menuItemsQueue.Count > 0 && _menuItemsStack.Peek().IsOK)
        {
            var item = _menuItemsQueue.Dequeue();
            Assert.NotNull(item);
            item.IsEnabled = true;
            _menuItemsStack.Push(item);
            Assert.True(_menuItemsStack.Peek().IsEnabled);
        }
    }
}
