namespace Coderama.Tests.DocumentStorage;

using Abstractions.Models;
using Api.Implementations.DocumentStorage;
using NSubstitute;
using ZiggyCreatures.Caching.Fusion;

public class FileStorageTests {

    [Fact]
    public async Task Test_FileStorage_CanStore_Success()
    {
        // Arrange
        (string Id, JsonDocument Document) request = (Guid.NewGuid().ToString(), JsonDocument.Parse("{}"));
        var cache = Substitute.For<IFusionCache>();

        var sut = new FileStorage(cache);

        // Act
        var result = await sut.StoreDocumentAsync(request.Id, request.Document, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
        result.AsT0.Value.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Test_FileStorage_CanGet_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var cache = new FusionCache(new FusionCacheOptions());
        var sut = new FileStorage(cache);

        // Act
        var result = await sut.GetDocumentByIdAsync(id, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
    }

    [Fact]
    public async Task Test_FileStorage_CanUpdate_Success()
    {
        // Arrange
        (string Id, JsonDocument Document) request = (Guid.NewGuid().ToString(), JsonDocument.Parse("{}"));
        var cache = Substitute.For<IFusionCache>();

        var sut = new FileStorage(cache);

        // Act
        var result = await sut.UpdateDocumentAsync(request.Id, request.Document, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
    }
}
