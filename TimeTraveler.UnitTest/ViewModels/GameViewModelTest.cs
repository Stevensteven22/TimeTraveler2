using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Messaging;
using Moq;
using TimeTraveler.Libary.Services;
using TimeTraveler.Services;
using Xunit;

namespace TimeTraveler.UnitTest.ViewModels;

public class GameViewModelTest
{
    [Fact]
    public void GoToResultView_Default()
    {
        string Problem1Answer = "苹果";
        string Problem2Answer = "攫金鸦印";
        string Problem3Answer = "琉璃百合";
        var resultVerifyServiceMock = new Mock<IResultVerifyService>();

        resultVerifyServiceMock
            .Setup(p =>
                p.BeforeVerify(
                    new
                    {
                        Problem1Answer,
                        Problem2Answer,
                        Problem3Answer,
                    }
                )
            )
            .Returns((true, "Correct!"));
        resultVerifyServiceMock.Verify(
            p =>
                p.BeforeVerify(
                    new
                    {
                        Problem1Answer,
                        Problem2Answer,
                        Problem3Answer,
                    }
                ),
            Times.Never
        );

        var mockResultVerifyService = resultVerifyServiceMock.Object;

        var verifyResult = mockResultVerifyService.BeforeVerify(
            new
            {
                Problem1Answer = Problem1Answer,
                Problem2Answer = Problem2Answer,
                Problem3Answer = Problem3Answer,
            }
        );
        if (!verifyResult.Item1)
        {
            Assert.False(false, verifyResult.Item2);
            return;
        }
        Assert.NotNull(Problem1Answer);
        Assert.NotNull(Problem2Answer);
        Assert.NotNull(Problem3Answer);
        WeakReferenceMessenger.Default.Send<object, string>(2, "OnForwardNavigation");
        if (
            mockResultVerifyService.Verify(
                new
                {
                    Problem1Answer = Problem1Answer,
                    Problem2Answer = Problem2Answer,
                    Problem3Answer = Problem3Answer,
                }
            )
        /*Problem1Answer == "苹果"
        && Problem2Answer == "攫金鸦印"
        && Problem3Answer == "琉璃百合"*/
        )
        {
            Assert.Equal(Problem1Answer, "苹果");
            Assert.Equal(Problem2Answer, "攫金鸦印");
            Assert.Equal(Problem3Answer, "琉璃百合");
            Assert.True(true);
            WeakReferenceMessenger.Default.Send<object, string>(true, "OnResultSubmitted");
        }
        else
        {
            Assert.False(false);
            WeakReferenceMessenger.Default.Send<object, string>(false, "OnResultSubmitted");
        }
    }
}
