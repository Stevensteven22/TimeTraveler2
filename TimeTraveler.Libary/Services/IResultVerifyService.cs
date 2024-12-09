namespace TimeTraveler.Libary.Services;

public interface IResultVerifyService
{
    public bool Verify(object result);
    
    public (bool,string) BeforeVerify(object result);
}