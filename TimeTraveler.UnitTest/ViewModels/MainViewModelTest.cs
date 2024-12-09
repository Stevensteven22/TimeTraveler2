using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Moq;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;
using Xunit;

namespace TimeTraveler.UnitTest.ViewModels;

public class MainViewModelTest
{
    [Fact]
    public void OnForwardNavigation_Default()
    {
        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockNavigationService = rootNavigationServiceMock.Object;
        var chapterNavigationServiceMock = new Mock<IChapterNavigationService>();
        var mockChapterNavigationService = chapterNavigationServiceMock.Object;

        int pageIndex = 1;
        ChapterViewModel _selectedMenuItem = new ChapterViewModel() { Id = 1 };
        var mainViewModel = new MainViewModel(mockChapterNavigationService, mockNavigationService);
        mainViewModel.IsNavigationEnabled = true;
        if (mainViewModel.IsNavigationEnabled && _selectedMenuItem != null)
        {
            var views = ChapterNavigationConstant.ChapterViews[_selectedMenuItem.Id];
            Assert.NotNull(views);
            mockChapterNavigationService.NavigateTo(views[pageIndex]);
        }
    }

    [Fact]
    public void OnBackHome_Default()
    {
        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockNavigationService = rootNavigationServiceMock.Object;
        var chapterNavigationServiceMock = new Mock<IChapterNavigationService>();
        var mockChapterNavigationService = chapterNavigationServiceMock.Object;

        ChapterViewModel _selectedMenuItem = new ChapterViewModel() { Id = 1 };
        var mainViewModel = new MainViewModel(mockChapterNavigationService, mockNavigationService);
        mainViewModel.IsNavigationEnabled = true;
        if (mainViewModel.IsNavigationEnabled && _selectedMenuItem != null)
        {
            mockNavigationService.NavigateTo(RootNavigationConstant.MainView);
        }
    }
    
    [Fact]
    public void OnMenuTapped_Default()
    {
        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockNavigationService = rootNavigationServiceMock.Object;
        var chapterNavigationServiceMock = new Mock<IChapterNavigationService>();
        var mockChapterNavigationService = chapterNavigationServiceMock.Object;

        var mainViewModel = new MainViewModel(mockChapterNavigationService, mockNavigationService);
        mainViewModel.IsNavigationEnabled = true;
        ChapterViewModel _selectedMenuItem = new ChapterViewModel() { Id = 1 };
        var views = ChapterNavigationConstant.ChapterViews[_selectedMenuItem.Id];
        Assert.NotNull(views);
        Assert.True(views.Count > 0, "The value should be greater than 0.");
        mockChapterNavigationService.NavigateTo(views[0]);
    }
    
}
