namespace Coderama.Tests.Idempotency;

using Abstractions.Contracts.Requests;
using Abstractions.Interfaces;
using Api.Implementations.DocumentRepository;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OneOf.Types;

public class DocumentIdempotencyTests {

    [Fact]
    public async Task Test_DocumentRepositoryCallsIdempotencyLayer_Success()
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
        await transactionRepository.Received(1).GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Test_DocumentRepositoryReturnsIdempotentResult_Success()
    {
        // Arrange
        var documentStorage = Substitute.For<IDocumentStorage>();
        documentStorage
            .StoreDocumentAsync(Arg.Any<string>(), Arg.Any<JsonDocument>(), Arg.Any<CancellationToken>())
            .Returns(new Success<string>(string.Empty));

        var document = Helpers.GetFakeDocument();
        var documentPostRequest = new DocumentPostRequest(document.InternalId, document.Tags, document.Data);

        var transactionRepository = Substitute.For<ITransactionRepository>();
        var transactionResult = TypedResults.Created("https://mock-tests.com/");
        transactionRepository.GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(transactionResult);

        var sut = new DocumentRepository(documentStorage, transactionRepository);

        // Act
        var result = sut.StoreDocumentAsync(documentPostRequest, CancellationToken.None);

        // Assert
        await result.ShouldNotThrowAsync();
        await documentStorage
            .DidNotReceive().StoreDocumentAsync(document.InternalId, document.Data, Arg.Any<CancellationToken>());
        await transactionRepository.Received(1).GetTransaction(Arg.Any<string>(), Arg.Any<CancellationToken>());
        (await result).ShouldBe(transactionResult);
    }
}
