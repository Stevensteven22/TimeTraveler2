namespace TimeTraveler.Libary.Services;

public interface IFlyService
{
    double Gravity { get; set; }
    double FlapStrength { get; set; }
    
    double UpdatePosition(double currentPosition, double velocity);// 根据当前的位置和速度更新新位置。
    double ApplyGravity(double velocity);// 应用重力影响更新速度。
    double Flap();// 触发一次跳跃，返回跳跃后的初始速度。


}