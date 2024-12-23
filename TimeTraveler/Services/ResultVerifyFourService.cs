using System.Collections.Generic;
using System.Linq;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Services;

public class ResultVerifyFourService : IResultVerifyFourService
{
    
    public string VerifyOrder(List<string> selectedOrder, List<string> correctOrder)
    {
        if (selectedOrder.SequenceEqual(correctOrder))
        {
            // 如果顺序正确，返回成功消息
            return "恭喜你完成任务！";
        }
        else
        {
            // 如果顺序错误，返回错误消息
            return "顺序错误，请重新开始";
        }
    }
    
    
    public void RestartGame(ref string gameStatus, ref int currentOrder, ref string button1Color, ref string button2Color, ref string button3Color, 
        ref string button4Color, ref string button5Color, ref string button6Color, ref string button7Color, ref string button8Color, 
        ref string button9Color, List<int> selectedOrder)
    {
        // 重置游戏状态
        gameStatus = "顺序错误，请重新开始!";
        selectedOrder.Clear();
        currentOrder = 0;
        
        // 重置按钮颜色
        button1Color = "Transparent";
        button2Color = "Transparent";
        button3Color = "Transparent";
        button4Color = "Transparent";
        button5Color = "Transparent";
        button6Color = "Transparent";
        button7Color = "Transparent";
        button8Color = "Transparent";
        button9Color = "Transparent";
    }
    
    public void ShowResultImage(ref bool imageSuccess)
    {
        // 加载成功后的图案，更新图像显示状态
        imageSuccess = true;
    }
}