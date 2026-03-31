namespace Coderama.Tests.DocumentStorage;

using Api.Implementations.DocumentStorage;

public class FileStorageTests {

    [Fact]
    public async Task Test_FileStorage_CanStore_Success()
    {
        // Arrange
        (string Id, JsonDocument Document) request = (Guid.NewGuid().ToString(), JsonDocument.Parse("{}"));

        var sut = new FileStorage();

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
        var Id = Guid.NewGuid().ToString();

        var sut = new FileStorage();

        // Act
        var result = await sut.GetDocumentByIdAsync(Id, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
    }

    [Fact]
    public async Task Test_FileStorage_CanUpdate_Success()
    {
        // Arrange
        (string Id, JsonDocument Document) request = (Guid.NewGuid().ToString(), JsonDocument.Parse("{}"));

        var sut = new FileStorage();

        // Act
        var result = await sut.UpdateDocumentAsync(request.Id, request.Document, CancellationToken.None);

        // Assert
        result.IsT0.ShouldBeTrue();
    }
}
