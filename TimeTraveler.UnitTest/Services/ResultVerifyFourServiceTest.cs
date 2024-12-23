using System.Collections.Generic;
using TimeTraveler.Services;
using Xunit;

namespace TimeTraveler.Tests
{
    public class ResultVerifyFourServiceTest
    {
        // Test the VerifyOrder method
        [Fact]
        public void VerifyOrder_ReturnsSuccessMessage_WhenOrderIsCorrect()
        {
            // Arrange
            var service = new ResultVerifyFourService();
            var selectedOrder = new List<string> { "Avatar1", "Avatar2", "Avatar3", "Avatar4" };
            var correctOrder = new List<string> { "Avatar1", "Avatar2", "Avatar3", "Avatar4" };

            // Act
            var result = service.VerifyOrder(selectedOrder, correctOrder);

            // Assert
            Assert.Equal("恭喜你完成任务！", result);
        }

        [Fact]
        public void VerifyOrder_ReturnsErrorMessage_WhenOrderIsIncorrect()
        {
            // Arrange
            var service = new ResultVerifyFourService();
            var selectedOrder = new List<string> { "Avatar1", "Avatar2", "Avatar3", "Avatar4" };
            var correctOrder = new List<string> { "Avatar4", "Avatar3", "Avatar2", "Avatar1" };

            // Act
            var result = service.VerifyOrder(selectedOrder, correctOrder);

            // Assert
            Assert.Equal("顺序错误，请重新开始", result);
        }

        // Test the RestartGame method
        [Fact]
        public void RestartGame_ShouldResetGameState()
        {
            // Arrange
            var service = new ResultVerifyFourService();
            var gameStatus = "Game in Progress";
            var currentOrder = 3;
            var selectedOrder = new List<int> { 1, 2, 3 };
            var button1Color = "Red";
            var button2Color = "Green";
            var button3Color = "Blue";
            var button4Color = "Yellow";
            var button5Color = "Orange";
            var button6Color = "Purple";
            var button7Color = "Pink";
            var button8Color = "Brown";
            var button9Color = "White";

            // Act
            service.RestartGame(ref gameStatus, ref currentOrder, ref button1Color, ref button2Color, ref button3Color, 
                ref button4Color, ref button5Color, ref button6Color, ref button7Color, ref button8Color, 
                ref button9Color, selectedOrder);

            // Assert
            Assert.Equal("顺序错误，请重新开始!", gameStatus);
            Assert.Empty(selectedOrder); // Ensure the selected order is cleared
            Assert.Equal(0, currentOrder); // Ensure current order is reset
            Assert.Equal("Transparent", button1Color); // Ensure button color reset
            Assert.Equal("Transparent", button2Color);
            Assert.Equal("Transparent", button3Color);
            Assert.Equal("Transparent", button4Color);
            Assert.Equal("Transparent", button5Color);
            Assert.Equal("Transparent", button6Color);
            Assert.Equal("Transparent", button7Color);
            Assert.Equal("Transparent", button8Color);
            Assert.Equal("Transparent", button9Color);
        }

        // Test the ShowResultImage method
        [Fact]
        public void ShowResultImage_ShouldSetImageSuccessToTrue()
        {
            // Arrange
            var service = new ResultVerifyFourService();
            bool imageSuccess = false;

            // Act
            service.ShowResultImage(ref imageSuccess);

            // Assert
            Assert.True(imageSuccess); // Ensure the image success is set to true
        }
    }
}
