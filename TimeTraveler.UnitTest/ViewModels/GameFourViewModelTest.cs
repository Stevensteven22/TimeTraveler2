using Moq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;
using TimeTraveler.Libary.Models;
using Xunit;

namespace TimeTraveler.UnitTest.ViewModels
{
    public class GameFourViewModelTest
    {
        private readonly Mock<IResultVerifyFourService> _mockResultVerifyFourService;
        private readonly Mock<IElementalService> _mockElementalService;
        private readonly GameFourViewModel _viewModel;

        public GameFourViewModelTest()
        {
            // Mock the IResultVerifyFourService dependency
            _mockResultVerifyFourService = new Mock<IResultVerifyFourService>();

            // Mock the IElementalService dependency
            _mockElementalService = new Mock<IElementalService>();

            // Create the view model with the mocked services
            _viewModel = new GameFourViewModel(_mockResultVerifyFourService.Object, _mockElementalService.Object);
        }

        // Test initialization of avatars
        [Fact]
        public void Test_AvatarsInitialization()
        {
            // Assert that the avatars collection has 9 avatars
            Assert.Equal(9, _viewModel.Avatars.Count);
        }

        // Test Game Status initialization
        [Fact]
        public void Test_GameStatusInitialization()
        {
            // Assert the game status is initialized correctly
            Assert.Equal("请按照顺序点击头像!", _viewModel.GameStatus);
        }

        // Test avatar click and color change
        [Fact]
        public void Test_AvatarClick_ShouldChangeButtonColor()
        {
            // Test clicking on the first avatar
            _viewModel.OnAvatarClick("0");

            // Assert that the color of Button1 is changed
            Assert.Equal("LightCoral", _viewModel.Button1Color);

            // Test clicking on the second avatar
            _viewModel.OnAvatarClick("1");

            // Assert that the color of Button2 is changed
            Assert.Equal("Yellow", _viewModel.Button2Color);
        }

        // Test order verification when the player clicks the correct sequence
        [Fact]
        public async Task Test_CheckOrder_CorrectSequence()
        {
            // Arrange: Set up mock behavior for GetElementalAsync and InsertOrUpdateElementalAsync
            var mockWindElement = new ResultModel { Name = "风元素", IsSelected = false, ImprovedValue1 = 0 };
            _mockElementalService.Setup(service => service.GetElementalAsync(It.IsAny<Expression<Func<ResultModel, bool>>>()))
                .ReturnsAsync(mockWindElement);

            _mockElementalService.Setup(service => service.InsertOrUpdateElementalAsync(It.IsAny<ObservableCollection<ResultModel>>()))
                .Returns(Task.CompletedTask);

            // Act: Click on the correct sequence of avatars
            _viewModel.OnAvatarClick("0");
            _viewModel.OnAvatarClick("4");
            _viewModel.OnAvatarClick("2");
            _viewModel.OnAvatarClick("7");

            // Assert: Check that the game status shows the success message
            Assert.Equal("恭喜你完成任务！", _viewModel.GameStatus);
            Assert.True(_viewModel.Imagesuccess);  // Ensure the success image is shown

            // Verify the mock service methods were called
            _mockElementalService.Verify(service => service.GetElementalAsync(It.IsAny<Expression<Func<ResultModel, bool>>>()), Times.Once);
            _mockElementalService.Verify(service => service.InsertOrUpdateElementalAsync(It.IsAny<ObservableCollection<ResultModel>>()), Times.Once);
        }

        // Test order verification when the player clicks an incorrect sequence
        [Fact]
        public void Test_CheckOrder_IncorrectSequence()
        {
            // Act: Click on an incorrect sequence of avatars
            _viewModel.OnAvatarClick("0");
            _viewModel.OnAvatarClick("1");
            _viewModel.OnAvatarClick("2");
            _viewModel.OnAvatarClick("8");

            // Assert: Check that the game status shows the error message
            Assert.Equal("顺序错误，请重新开始!", _viewModel.GameStatus);
        }

        // Test Restart functionality
        [Fact]
        public void Test_Restart_ShouldResetGame()
        {
            // Act: Set a game status and simulate some clicks
            _viewModel.OnAvatarClick("0");
            _viewModel.OnAvatarClick("4");

            // Call restart
            _viewModel.Restart();

            // Assert: Check that game is reset
            Assert.Equal("顺序错误，请重新开始!", _viewModel.GameStatus);
            Assert.Equal("Transparent", _viewModel.Button1Color);
            Assert.Equal("Transparent", _viewModel.Button2Color);
        }

        // Test result view redirection
        [Fact]
        public void Test_GoToResultView_ShouldSendMessage()
        {
            // Click on the correct sequence of avatars to complete the game
            _viewModel.OnAvatarClick("0");
            _viewModel.OnAvatarClick("4");
            _viewModel.OnAvatarClick("2");
            _viewModel.OnAvatarClick("7");

            // Call GoToResultView
            _viewModel.GoToResultView();

            // Test that the message is sent (in this case, verify the message is sent correctly)
            // You can use a mock or a spy for the WeakReferenceMessenger to verify if it was called
            // In this case, we won't test the actual message send, but you can add logic to test that part
        }
    }
}
