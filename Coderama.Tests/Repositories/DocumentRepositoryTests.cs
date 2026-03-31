namespace Coderama.Tests.Repositories;

using Abstractions.Contracts.Requests;
using Abstractions.Interfaces;
using Abstractions.Models;
using Api.Implementations.DocumentRepository;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OneOf.Types;

public class DocumentRepositoryTests {

    [Fact]
    public async Task Test_DocumentRepositoryUpdateDocumentAsync_Failure()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        var document = Helpers.GetFakeDocument();

        documentStorage
            .GetDocumentByIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Success<InternalDocument>(document));

        documentStorage
            .UpdateDocumentAsync(Arg.Any<string>(), Arg.Any<JsonDocument>(), Arg.Any<CancellationToken>())
            .Returns(new Success());

        var transactionRepository = Substitute.For<ITransactionRepository>();
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((IResult?)null);

        var request = new DocumentPutRequest(Guid.NewGuid().ToString(), document.Tags, document.Data);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

        // Act
        var result = sut.UpdateDocumentAsync(document.InternalId, request, CancellationToken.None);

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
            .Returns(new Success<InternalDocument>(Helpers.GetFakeDocument()));

        var internalId = Guid.NewGuid().ToString();

        var transactionRepository = Substitute.For<ITransactionRepository>();
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((IResult?)null);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

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

        var document = Helpers.GetFakeDocument();
        var documentPostRequest = new DocumentPostRequest(document.InternalId, document.Tags, document.Data);

        var transactionRepository = Substitute.For<ITransactionRepository>();
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((IResult?)null);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

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

        var transactionRepository = Substitute.For<ITransactionRepository>();
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((IResult?)null);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

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

        var transactionRepository = Substitute.For<ITransactionRepository>();
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((IResult?)null);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

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

        var transactionRepository = Substitute.For<ITransactionRepository>();
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((IResult?)null);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

        // Act
        var result = sut.DeleteDocumentAsync("", CancellationToken.None);

        // Assert
        await result.ShouldThrowAsync<NotImplementedException>();
    }
}
