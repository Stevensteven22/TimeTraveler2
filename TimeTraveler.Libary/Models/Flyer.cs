namespace TimeTraveler.Libary.Models;

public class Flyer
{
    public double X { get; set; }  // 矩形的X坐标
    public double Y { get; set; }  // 矩形的Y坐标
    public double Width { get; set; }  // 矩形的宽度
    public double Height { get; set; }  // 矩形的高度
    public double Velocity { get; set; } // 小球的速度

    private readonly double _gravity = 0.3;  // 重力
    private readonly double _flapStrength = -8; // 跳跃力度


    
    public Flyer(double initialX, double initialY,double width, double height)
    {
        X = initialX;
        Y = initialY;
        Width = width;
        Height = height;
        Velocity = 0;
    }
    
    // 更新小球的速度和位置
    public void UpdatePosition()
    {
        Velocity += _gravity;  // 应用重力
        Y += Velocity;  // 更新小球的Y坐标
    }

    // 触发跳跃
    public void Flap()
    {
        Velocity = _flapStrength;
    }

    // 碰撞检测：检查小球是否触底
    public bool CheckCollision(double groundY)
    {
        if (Y + Height > groundY) // 检查是否触底
        {
            Y = groundY - Height; // 限制小球位置不掉出底部
            Velocity = 0; // 停止下落
            return true;
        }
        return false;
    }
}