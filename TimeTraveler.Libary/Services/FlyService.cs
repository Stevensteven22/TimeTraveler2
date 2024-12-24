namespace TimeTraveler.Libary.Services;

public class FlyService : IFlyService
{
    public double Gravity { get; set; } = 0.5;
    public double FlapStrength { get; set; } = -10;

    public double UpdatePosition(double currentPosition, double velocity)
    {
        return currentPosition + velocity;
    }

    public double ApplyGravity(double velocity)
    {
        return velocity + Gravity;
    }

    public double Flap()
    {
        return FlapStrength;
    }

}