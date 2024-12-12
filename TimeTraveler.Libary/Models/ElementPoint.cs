namespace TimeTraveler.Libary.Models;

public class ElementPoint
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public bool IsPickedUp { get; set; }
    
    
    public ElementPoint(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    
    public void UpdatePosition1(double speed)
    {
        X -= speed; // 每次更新时障碍物向左移动
       

        // 确保障碍物在屏幕左侧时重新生成
        if (X + Width < 0 && IsPickedUp == false)
        {
            X = 1200; // 假设屏幕宽度为 800
            if (new Random().Next(0, 2) == 0)
            {
                Y = 300; // 上边贴紧屏幕
            }
            else
            {
                Y = 450; // 下边贴紧屏幕
            }
        }
    }
    
    public void UpdatePosition2(double speed)
    {
        X -= speed; // 每次更新时障碍物向左移动
       

        // 确保障碍物在屏幕左侧时重新生成
        if (X + Width < 0 && IsPickedUp == false)
        {
            X = 1200; // 假设屏幕宽度为 800
            Y = new Random().Next(0, 550);

        }
    }
    
    public void UpdatePosition3(double speed)
    {
        X -= speed; // 每次更新时障碍物向左移动

        if (X + Width < 0 && IsPickedUp == false)
        {
            X = 1200;
        }
    }

    public void PickUp()
    {
        IsPickedUp = true; // 设置为已拾取
    }
    
}