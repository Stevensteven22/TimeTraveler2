namespace TimeTraveler.Libary.Services;

public interface IResultVerifyFourService
{
    public string VerifyOrder(List<string> selectedOrder, List<string> correctOrder);

    public void RestartGame(ref string gameStatus, ref int currentOrder, ref string button1Color,
        ref string button2Color, ref string button3Color,
        ref string button4Color, ref string button5Color, ref string button6Color, ref string button7Color,
        ref string button8Color,
        ref string button9Color, List<int> selectedOrder);

    public void ShowResultImage(ref bool imageSuccess);
}