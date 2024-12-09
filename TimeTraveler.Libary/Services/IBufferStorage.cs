using System.Linq.Expressions;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

public interface IBuffStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Buff> GetBuffAsync(int id);

    Task<Buff> GetBuffAsync(Expression<Func<Buff, bool>> where);

    Task<IList<Buff>> GetBuffsAsync(Expression<Func<Buff, bool>> where, int skip, int take);

    Task<bool> RemoveAllBuffAsync();

    Task<bool> AddBuffsAsync(params Buff[] buffs);

    Task<bool> SaveBuffsAsync(params Buff[] buffs);
}

public class BuffStorageUpdatedEventArgs : EventArgs
{
    public Buff UpdatedBuff { get; }

    public BuffStorageUpdatedEventArgs(Buff buff)
    {
        UpdatedBuff = buff;
    }
}
