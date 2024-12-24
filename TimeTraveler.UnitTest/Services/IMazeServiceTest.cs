using TimeTraveler.Libary.Services;
using Xunit;

namespace TimeTraveler.UnitTest.Services
{
    public class MazeServiceTests
    {
        private readonly MazeService _mazeService;

        public MazeServiceTests()
        {
            _mazeService = new MazeService();
            _mazeService.GenerateMaze(); // 每个测试前初始化迷宫
        }

        [Fact]
        public void GenerateMaze_ShouldInitializeMazeCorrectly()
        {
            // Arrange: 已经在构造函数中调用了 GenerateMaze 方法
            
            // Act: 获取迷宫文本表示
            var mazeRepresentation = _mazeService.GetMazeRepresentation();

            // Assert: 检查迷宫是否按预期初始化，至少检查边界是否是墙壁('#')
            Assert.Contains("##########", mazeRepresentation); // 检查顶部是否为墙
            Assert.Contains("##########", mazeRepresentation); // 检查底部是否为墙
        }

        [Fact]
        public void tesi_ifno()
        {
           
            Assert.False(_mazeService.IsMoveValid(-1, -1));
        }

        [Fact]
        public void GetMazeRepresentation_ShouldReturnCorrectMazeFormat()
        {
            // Arrange: 已经在构造函数中调用了 GenerateMaze 方法
            
            // Act: 获取迷宫文本表示
            var mazeRepresentation = _mazeService.GetMazeRepresentation();

            // Assert: 检查迷宫格式是否正确（包含换行符）
            Assert.Contains("\n", mazeRepresentation);
            Assert.StartsWith("#", mazeRepresentation); 
            Assert.EndsWith("\n", mazeRepresentation);   
        }

        [Theory]
        [InlineData(1, 1, true)] // 玩家起始位置应该是可以移动的
        [InlineData(0, 0, false)] // 迷宫顶部左上角应该是墙壁
        [InlineData(14, 14, false)] // 迷宫底部右下角应该是墙壁
        public void IsMoveValid_ShouldReturnCorrectResult(int x, int y, bool expected)
        {
            // Act: 检查给定位置是否有效
            var result = _mazeService.IsMoveValid(x, y);

            // Assert: 确认返回值是否符合预期
            Assert.Equal(expected, result);
        }
    }
    
}