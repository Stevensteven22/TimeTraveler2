using System.Linq.Expressions;
using SQLite;
using TimeTraveler.Libary.Helpers;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

public class BuffStorage : IBuffStorage
{
    public const int NumberPoetry = 5;

    public const string DbName = $"{nameof(TimeTraveler)}db.sqlite3";

    public static readonly string TimeTravelerDbPath = PathHelper.GetLocalFilePath(DbName);

    private SQLiteAsyncConnection _connection;

    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(TimeTravelerDbPath);

    private readonly IPreferenceStorage _preferenceStorage;

    public BuffStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }

    public event EventHandler<BuffStorageUpdatedEventArgs>? Updated;
    public bool IsInitialized =>
        _preferenceStorage.Get(BuffStorageConstant.VersionKey, default(int))
        == BuffStorageConstant.Version;

    public async Task InitializeAsync()
    {
        await using var dbFileStream = new FileStream(TimeTravelerDbPath, FileMode.OpenOrCreate);
        await using var dbAssetStream =
            typeof(BuffStorage).Assembly.GetManifestResourceStream(DbName)
            ?? throw new Exception($"Manifest not found: {DbName}");
        await dbAssetStream.CopyToAsync(dbFileStream);

        _preferenceStorage.Set(BuffStorageConstant.VersionKey, BuffStorageConstant.Version);
    }

    public async Task<bool> CreateTableAsync()
    {
        var createTableResult = await Connection.CreateTableAsync<Buff>();
        return createTableResult == CreateTableResult.Created;
    }

    public Task<Buff> GetBuffAsync(int id) =>
        Connection.Table<Buff>().FirstOrDefaultAsync(p => p.Id == id);

    public Task<Buff> GetBuffAsync(Expression<Func<Buff, bool>> where) =>
        Connection.Table<Buff>().FirstOrDefaultAsync(where);

    public async Task<IList<Buff>> GetBuffsAsync(
        Expression<Func<Buff, bool>> where,
        int skip,
        int take
    ) => await Connection.Table<Buff>().Where(where).Skip(skip).Take(take).ToListAsync();

    public async Task<bool> RemoveAllBuffAsync()
    {
        bool result = false;
        try
        {
            await Connection.DeleteAllAsync<Buff>();
        }
        catch (Exception ex)
        {
            return false;
        }
        result = true;
        return result;
    }

    public async Task<bool> AddBuffsAsync(params Buff[] buffs)
    {
        bool result = false;
        try
        {
            foreach (var buff in buffs)
            {
                buff.CreatedAt = DateTime.Now;
                buff.UpdatedAt = DateTime.Now;
            }
            await Connection.InsertAllAsync(buffs);
        }
        catch (Exception ex)
        {
            return false;
        }
        result = true;
        return result;
    }

    public async Task<bool> SaveBuffsAsync(params Buff[] buffs)
    {
        bool result = false;
        try
        {
            foreach (var buff in buffs)
            {
                buff.CreatedAt = DateTime.Now;
                buff.UpdatedAt = DateTime.Now;
            }
            await Connection.UpdateAllAsync(buffs);
        }
        catch (Exception ex)
        {
            return false;
        }
        result = true;
        return result;
    }

    public async Task CloseAsync() => await Connection.CloseAsync();
}

public static class BuffStorageConstant
{
    public const string VersionKey = nameof(BuffStorageConstant) + "." + nameof(Version);

    public const int Version = 1;
}
