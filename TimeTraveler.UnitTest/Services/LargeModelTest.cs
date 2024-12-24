using Moq;
using RestSharp;
using Newtonsoft.Json;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.Models;
using System;
using Xunit;

namespace TimeTraveler.Tests
{
    public class LargeModelServiceTests
    {
        private readonly Mock<IRestClient> _mockRestClient;
        private readonly Mock<IRestResponse> _mockResponse;
        private LargeModelService _largeModelService;

        const string API_KEY = "dCa7niTOlzuoTDpp99I6Lgh6";
        const string SECRET_KEY = "eRlq4eQ1PfFerHCKgHHX35oUZyXMefl8";

        private const string TokenUrl = "https://aip.baidubce.com/oauth/2.0/token";
        private const string ApiUrl = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions_pro";
        public LargeModelServiceTests()
        {
            // 初始化模拟对象
            _mockRestClient = new Mock<IRestClient>();
            _mockResponse = new Mock<IRestResponse>();
            _largeModelService = new LargeModelService();
        }

        [Fact]
        public void GetAccessToken_ShouldReturnToken_WhenApiCallIsSuccessful()
        {
            // Arrange
            string mockResponseContent = "{\"access_token\":\"mock_access_token\"}";
            _mockResponse.Setup(r => r.IsSuccessful).Returns(true);
            _mockResponse.Setup(r => r.Content).Returns(mockResponseContent);
            _mockRestClient.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(_mockResponse.Object);

            // Act
            string token = _largeModelService.GetAccessToken();

            // Assert
            Assert.NotNull(token);
        }

       


        [Fact]
        public void GeneratePuzzle_ShouldReturnPuzzle_WhenApiCallIsSuccessful()
        {
            // Arrange
            string mockResponseContent = "{\"result\":\"谜题描述 A:选项1 B:选项2 C:选项3 正确答案: B 解释: 这是解释\"}";
            _mockResponse.Setup(r => r.IsSuccessful).Returns(true);
            _mockResponse.Setup(r => r.Content).Returns(mockResponseContent);
            _mockRestClient.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(_mockResponse.Object);

            // Act
            Puzzle puzzle = _largeModelService.GeneratePuzzle("Math");

            // Assert
            Assert.NotNull(puzzle);
           
        }

       

      

        [Fact]
        public void ParseResponse_ShouldReturnNull_WhenResponseIsInvalid()
        {
            // Arrange
            string responseContent = "{\"invalid\":\"response\"}";

            // Act
            Puzzle puzzle = LargeModelService.ParseResponse(responseContent);

            // Assert
            Assert.Null(puzzle);
        }

       

        [Fact]
        public void ExtractQuestion_ShouldReturnCorrectQuestion()
        {
            // Arrange
            string questionAndOptions = "谜题描述 A:选项1 B:选项2 C:选项3";

            // Act
            string question = LargeModelService.ExtractQuestion(questionAndOptions);

            // Assert
            Assert.Equal("谜题描述", question);
        }

        [Fact]
        public void ExtractCorrectAnswer_ShouldReturnCorrectAnswer()
        {
            // Arrange
            string correctAnswerAndExplanation = "B 解释内容";

            // Act
            string correctAnswer = LargeModelService.ExtractCorrectAnswer(correctAnswerAndExplanation);

            // Assert
            Assert.Equal("B", correctAnswer);
        }

        [Fact]
        public void ExtractExplanation_ShouldReturnExplanation()
        {
            // Arrange
            string correctAnswerAndExplanation = "B 解释内容";

            // Act
            string explanation = LargeModelService.ExtractExplanation(correctAnswerAndExplanation, "B");

            // Assert
            Assert.Equal("解释内容", explanation);
        }
        
         // 测试 ExtractOptions 方法
        [Fact]
        public void ExtractOptions_ShouldReturnCorrectOptions()
        {
            // Arrange
            string questionAndOptions = "谜题描述 A:选项1 B:选项2 C:选项3";

            // Act
            string[] options = LargeModelService.ExtractOptions(questionAndOptions);

            // Assert
            Assert.Equal(3, options.Length);
            Assert.Equal("A:选项1", options[0]);
            Assert.Equal("B:选项2", options[1]);
            Assert.Equal("C:选项3", options[2]);
        }

        [Fact]
        public void ExtractOptions_ShouldReturnEmptyArray_WhenNoOptions()
        {
            // Arrange
            string questionAndOptions = "谜题描述";

            // Act
            string[] options = LargeModelService.ExtractOptions(questionAndOptions);

            // Assert
            Assert.Empty(options);
        }
      // 测试 ExtractQuestion 方法   
       

        [Fact]
        public void ExtractQuestion_ShouldReturnWholeString_WhenNoOptions()
        {
            // Arrange
            string questionAndOptions = "谜题描述";

            // Act
            string question = LargeModelService.ExtractQuestion(questionAndOptions);

            // Assert
            Assert.Equal("谜题描述", question);
        }

        // 测试 ExtractCorrectAnswer 方法
       

        [Fact]
        public void ExtractCorrectAnswer_ShouldReturnEmpty_WhenNoAnswer()
        {
            // Arrange
            string correctAnswerAndExplanation = "解释内容";

            // Act
            string correctAnswer = LargeModelService.ExtractCorrectAnswer(correctAnswerAndExplanation);

            // Assert
            Assert.Empty(correctAnswer);
        }

        // 测试 ExtractExplanation 方法
        [Fact]
        public void ExtractExplanation_ShouldReturnCorrectExplanation()
        {
            // Arrange
            string correctAnswerAndExplanation = "B 解释内容";

            // Act
            string explanation = LargeModelService.ExtractExplanation(correctAnswerAndExplanation, "B");

            // Assert
            Assert.Equal("解释内容", explanation);
        }

        [Fact]
        public void ExtractExplanation_ShouldReturnEmpty_WhenNoExplanation()
        {
            // Arrange
            string correctAnswerAndExplanation = "B";

            // Act
            string explanation = LargeModelService.ExtractExplanation(correctAnswerAndExplanation, "B");

            // Assert
            Assert.Empty(explanation);
        }
    }
}
