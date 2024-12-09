using Moq;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.UnitTest.Helpers;

public class BuffStorageHelper {
    public static void RemoveDatabaseFile() =>
        File.Delete(BuffStorage.TimeTravelerDbPath);

    public static async Task<BuffStorage> GetInitializedBuffStorage() {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p =>
            p.Get(BuffStorageConstant.VersionKey, -1)).Returns(-1);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var buffStorage = new BuffStorage(mockPreferenceStorage);
        await buffStorage.InitializeAsync();
        return buffStorage;
    }
}