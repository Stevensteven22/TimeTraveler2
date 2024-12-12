using Moq;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace TimeTraveler.UnitTest.ViewModels;


public class GameThreeViewModelTests
{
    private readonly Mock<IFlyService> _mockFlyService;
    private readonly Mock<IElementalService> _mockElementalService;
    private readonly GameThreeViewModel _viewModel;


    public GameThreeViewModelTests()
    {
        _mockFlyService = new Mock<IFlyService>();
        _viewModel = new GameThreeViewModel(_mockFlyService.Object, _mockElementalService.Object);

    }

    [Fact]
    public void Flap_WhenGameIsNotOver_ShouldUpdateBallPosition()
    {
        _viewModel.ResetGame();
        double initialBallY = _viewModel.BallY;
        
        _viewModel.Flap();
        _viewModel.Update();
        
        Assert.NotEqual(initialBallY, _viewModel.BallY); 
    }

    [Fact]
    public void Flap_WhenGameIsOver_ShouldNotUpdateBallPosition()
    {
        _viewModel.ResetGame();
        _viewModel.GameOver(); 
        double initialBallY = _viewModel.BallY;
        
        _viewModel.Flap();
        _viewModel.Update();
        
        Assert.Equal(initialBallY, _viewModel.BallY); // 游戏结束后小球位置应不变
    }
    
    // 测试游戏重置
    [Fact]
    public void ResetGame_ShouldResetGameState()
    {
        _viewModel.ResetGame();

        // 验证游戏重置后的状态
        Assert.False(_viewModel.IsGameOver);
        Assert.False(_viewModel.IsGameWon);
        Assert.Equal(100, _viewModel.BallY);  // 期望小球 Y 位置为初始位置
    }
    
    [Fact]
    public void CheckCollision_ShouldReturnTrueIfBallCollidesWithObstacle()
    {
        _viewModel.ResetGame();

        // 模拟小球与障碍物碰撞
        var ballX = _viewModel.BallX;
        var ballY = _viewModel.BallY;
        _viewModel._obstacle1 = new Obstacle(ballX, ballY, 100, 100);  // 将障碍物放在小球的当前位置

        var result = _viewModel.CheckCollision();

        Assert.True(result);  // 期望返回 true，表示发生了碰撞
    }

    // 测试 TimerTick 方法
    [Fact]
    public void TimerTick_ShouldUpdateTimeElapsedAndTriggerWinWhenTimeExpires()
    {
        _viewModel.ResetGame();
        
        // 模拟游戏时间达到设定的胜利时间
        _viewModel.TimerTick();  // 每调用一次更新一次
        _viewModel._timeElapsedInSeconds = 20.0;  // 设置时间已到30秒

        _viewModel.TimerTick();

        Assert.True(_viewModel.IsGameWon);  // 期望游戏获胜
    }
    
    // 测试游戏胜利
    [Fact]
    public void GameWon_ShouldSetIsGameWonTrueAndStopTimer()
    {
        _viewModel.ResetGame();

        // 调用 GameWon 方法
        _viewModel.GameWon();

        // 验证游戏胜利状态
        Assert.True(_viewModel.IsGameWon);
        Assert.True(_viewModel.IsGameOver);
    }

    // 测试跳跃时小球的速度是否变化
    [Fact]
    public void Flap_ShouldChangeBallVelocity()
    {
        _viewModel.ResetGame();
        
        var initialVelocity = _viewModel._ball.Velocity;

        // 执行 Flap 命令
        _viewModel.FlapCommand.Execute(null);

        // 验证小球的速度发生了变化
        Assert.NotEqual(initialVelocity, _viewModel._ball.Velocity);
    }

    
}
