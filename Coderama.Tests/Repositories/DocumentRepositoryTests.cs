namespace Coderama.Tests.Repositories;

using Abstractions.Contracts.Requests;
using Abstractions.Interfaces;
using Abstractions.Models;
using Api.Implementations.DocumentRepository;
using NSubstitute;
using OneOf.Types;

public class DocumentRepositoryTests {

    [Fact]
    public async Task Test_DocumentRepositoryUpdateDocumentAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        var document = GetFakeDocument();

        documentStorage
            .GetDocumentByIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Success<InternalDocument>(document));

        documentStorage
            .UpdateDocumentAsync(Arg.Any<string>(), Arg.Any<JsonDocument>(), Arg.Any<CancellationToken>())
            .Returns(new Success());


        var sut = new DocumentRepository(documentStorage);

        // Act
        var result = sut.UpdateDocumentAsync(document.InternalId, document.Data, CancellationToken.None);

        // Assert
        await result.ShouldNotThrowAsync();
        await documentStorage
            .Received(1).GetDocumentByIdAsync(document.InternalId, Arg.Any<CancellationToken>());
        await documentStorage
            .Received(1).UpdateDocumentAsync(document.InternalId, document.Data, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Test_DocumentRepositoryGetDocumentByIdAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        documentStorage
            .GetDocumentByIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Success<InternalDocument>(GetFakeDocument()));

        var internalId = Guid.NewGuid().ToString();

        var sut = new DocumentRepository(documentStorage);

        // Act
        var result = sut.GetDocumentByIdAsync(internalId, CancellationToken.None);

        // Assert
        await result.ShouldNotThrowAsync();
        await documentStorage
            .Received(1).GetDocumentByIdAsync(internalId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Test_DocumentRepositoryStoreDocumentAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        documentStorage
            .StoreDocumentAsync(Arg.Any<string>(), Arg.Any<JsonDocument>(), Arg.Any<CancellationToken>())
            .Returns(new Success<string>(string.Empty));

        var document = GetFakeDocument();
        var documentPostRequest = new DocumentPostRequest(document.InternalId, document.Tags, document.Data);

        var sut = new DocumentRepository(documentStorage);

        // Act
        var result = sut.StoreDocumentAsync(documentPostRequest, CancellationToken.None);

        // Assert
        await result.ShouldNotThrowAsync();
        await documentStorage
            .Received(1).StoreDocumentAsync(document.InternalId, document.Data, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Test_DocumentRepositoryGetAllDocumentsAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        var sut = new DocumentRepository(documentStorage);

        // Act
        var result = sut.GetAllDocumentsAsync(CancellationToken.None);

        // Assert
        await result.ShouldThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task Test_DocumentRepositorySearchDocumentByIdAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        var sut = new DocumentRepository(documentStorage);

        // Act
        var result = sut.SearchDocumentByIdAsync("", CancellationToken.None);

        // Assert
        await result.ShouldThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task Test_DocumentRepositoryDeleteDocumentAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        var sut = new DocumentRepository(documentStorage);

        // Act
        var result = sut.DeleteDocumentAsync("", CancellationToken.None);

        // Assert
        await result.ShouldThrowAsync<NotImplementedException>();
    }

    private static readonly string[] AttrNames = ["name", "path", "user", "comment", "source", "type"];
    private static readonly char[] Charset = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    private InternalDocument GetFakeDocument()
    {
        return new InternalDocument(
            Guid.NewGuid().ToString("D"),
            [],
            JsonDocument.Parse(
            $$"""
              {
                "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}",
                "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}"
              }
              """)
        );
    }
}
