using TimeTraveler.Libary.Services;
using Xunit;

namespace TimeTraveler.UnitTest.Services;

public class FlyServiceTest
{
    private readonly FlyService _flyService;

    public FlyServiceTest()
    {
        _flyService = new FlyService();
    }

    [Fact]
    public void UpdatePosition_WhenCalled_ShouldUpdatePositionCorrectly()
    {
        double initialPosition = 100;
        double velocity = 5; // 假设速度是5
        
        double newPosition = _flyService.UpdatePosition(initialPosition, velocity);
        
        Assert.Equal(105, newPosition); // 100 + 5 = 105
    }

    [Fact]
    public void ApplyGravity_WhenCalled_ShouldUpdateVelocityCorrectly()
    {
        double initialVelocity = 10;
        
        double newVelocity = _flyService.ApplyGravity(initialVelocity);
        
        Assert.Equal(10.5, newVelocity); // 10 + 0.5 (gravity) = 10.5
    }

    [Fact]
    public void Flap_WhenCalled_ShouldReturnFlapStrength()
    {
        double flapStrength = _flyService.Flap();
        
        Assert.Equal(-10, flapStrength); // 检查 Flap() 方法返回的是 -10
    }
    
    // 测试 Gravity 属性的 Get 和 Set
    [Fact]
    public void Gravity_GetAndSet_ShouldReturnExpectedValue()
    {
        // 1. 测试 get 操作，确保初始值为默认值 0.5
        Assert.Equal(0.5, _flyService.Gravity);

        // 2. 测试 set 操作，修改 Gravity 属性值为 1.0
        _flyService.Gravity = 1.0;
        Assert.Equal(1.0, _flyService.Gravity);
    }

    // 测试 FlapStrength 属性的 Get 和 Set
    [Fact]
    public void FlapStrength_GetAndSet_ShouldReturnExpectedValue()
    {
        // 1. 测试 get 操作，确保初始值为默认值 -10
        Assert.Equal(-10, _flyService.FlapStrength);

        // 2. 测试 set 操作，修改 FlapStrength 属性值为 -15
        _flyService.FlapStrength = -15;
        Assert.Equal(-15, _flyService.FlapStrength);
    }
}