using System.Linq.Expressions;
using Moq;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;
using TimeTraveler.UnitTest.Helpers;
using Xunit;
using static TimeTraveler.Libary.Services.BuffStorageConstant;

namespace TimeTraveler.UnitTest.Services;

public class BuffStorageTest : IDisposable
{
    public BuffStorageTest() => BuffStorageHelper.RemoveDatabaseFile();

    public void Dispose() => BuffStorageHelper.RemoveDatabaseFile();

    [Fact]
    public async Task CreateTableAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var BuffStorage = new BuffStorage(mockPreferenceStorage);

        var result = await BuffStorage.CreateTableAsync();
        Assert.Equal(true, result);
        await BuffStorage.CloseAsync();
    }

    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock
            .Setup(p => p.Get(VersionKey, default(int)))
            .Returns(BuffStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var BuffStorage = new BuffStorage(mockPreferenceStorage);
        Assert.True(BuffStorage.IsInitialized);

        preferenceStorageMock.Verify(p => p.Get(VersionKey, default(int)), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var BuffStorage = new BuffStorage(mockPreferenceStorage);

        Assert.False(File.Exists(BuffStorage.TimeTravelerDbPath));
        await BuffStorage.InitializeAsync();
        Assert.True(File.Exists(BuffStorage.TimeTravelerDbPath));

        preferenceStorageMock.Verify(
            p => p.Set(VersionKey, BuffStorageConstant.Version),
            Times.Once
        );
    }

    [Fact]
    public async Task GetBuffAsync_Default()
    {
        var BuffStorage = await BuffStorageHelper.GetInitializedBuffStorage();
        await BuffStorage.AddBuffsAsync(
            new Buff
            {
                Id = 1,
                Name = "火元素",
                Description = "攻击力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            }
        );
        var buff = await BuffStorage.GetBuffAsync(1);
        Assert.Equal("火元素", buff.Name);
        await BuffStorage.CloseAsync();
    }

    [Fact]
    public async Task GetBuffsAsync_Default()
    {
        var BuffStorage = await BuffStorageHelper.GetInitializedBuffStorage();
        var result = await BuffStorage.AddBuffsAsync(
            new Buff
            {
                Name = "火元素",
                Description = "攻击力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "冰元素",
                Description = "生命力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "风元素",
                Description = "闪避率+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "雷元素",
                Description = "暴击率+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "岩元素",
                Description = "防御力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            }
        );
        var buffs = await BuffStorage.GetBuffsAsync(
            Expression.Lambda<Func<Buff, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Buff), "p")
            ),
            0,
            int.MaxValue
        );
        Assert.Equal(BuffStorage.NumberPoetry, buffs.Count());
        await BuffStorage.CloseAsync();
    }

    [Fact]
    public async Task AddBuffAsync_Default()
    {
        var BuffStorage = await BuffStorageHelper.GetInitializedBuffStorage();
        var result = await BuffStorage.AddBuffsAsync(
            new Buff
            {
                Name = "火元素",
                Description = "攻击力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "冰元素",
                Description = "生命力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "风元素",
                Description = "闪避率+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "雷元素",
                Description = "暴击率+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "岩元素",
                Description = "防御力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            }
        );
        Assert.Equal(true, result);
        await BuffStorage.CloseAsync();
    }

    [Fact]
    public async Task RemoveAllBuffAsync_Default()
    {
        var BuffStorage = await BuffStorageHelper.GetInitializedBuffStorage();
        var result = await BuffStorage.RemoveAllBuffAsync();
        Assert.Equal(true, result);
        await BuffStorage.CloseAsync();
    }

    [Fact]
    public async Task SaveBuffsAsync_Default()
    {
        var BuffStorage = await BuffStorageHelper.GetInitializedBuffStorage();
        await BuffStorage.AddBuffsAsync(
            new Buff
            {
                Name = "火元素",
                Description = "攻击力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "冰元素",
                Description = "生命力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "风元素",
                Description = "闪避率+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "雷元素",
                Description = "暴击率+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new Buff
            {
                Name = "岩元素",
                Description = "防御力+10%",
                Value1 = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            }
        );
        var buff = await BuffStorage.GetBuffAsync(x => x.Name == "火元素");
        buff.Value1 = 30;
        var result = await BuffStorage.SaveBuffsAsync(buff);
        var newBuff = await BuffStorage.GetBuffAsync(x => x.Name == "火元素");
        Assert.Equal(newBuff.Value1, 30);
        await BuffStorage.CloseAsync();
    }
}
