namespace TimeTraveler.Libary.Services;

public interface IGameTBSService:IDisposable
{
    public void Produce(object data);

    public EventHandler<EventArgs> OnConsumed { get; set; }

    public EventHandler<EventArgs> OnProduced { get; set; }
}
