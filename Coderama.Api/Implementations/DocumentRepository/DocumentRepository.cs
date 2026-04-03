namespace Coderama.Api.Implementations.DocumentRepository;

using Abstractions.Contracts.Requests;
using Abstractions.Contracts.Responses;
using Mappers;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "AsyncMethodWithoutAwait")]
public class DocumentRepository(IDocumentStorage documentStorage, ITransactionRepository transactions) : IDocumentRepository {

    public async Task<IResult> GetAllDocumentsAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    public async Task<IResult> SearchDocumentByIdAsync(string internalId, CancellationToken cancellationToken) => throw new NotImplementedException();

    public async Task<IResult> GetDocumentByIdAsync(string internalId, CancellationToken cancellationToken)
    {
        var getResult = await documentStorage.GetDocumentByIdAsync(internalId, cancellationToken);

        var mapper = new DocumentGetByIdMapper();

        return getResult.Match<IResult>(
            success => {
                var result = mapper.MapToResponse(success.Value);
                return TypedResults.Ok(result);
            },
            notfound => TypedResults.NotFound()
        );
    }

    public async Task<IResult> StoreDocumentAsync(DocumentPostRequest request, CancellationToken cancellationToken)
    {
        var transaction = await transactions.GetTransaction(request.Id, cancellationToken);

        if (transaction is not null) { return transaction; }

        var storeResult = await documentStorage.StoreDocumentAsync(request.Id, request.Data, cancellationToken);

        return await storeResult.Match<Task<IResult>>(
            async created => {
                var result = TypedResults.CreatedAtRoute(
                    routeName: "GetDocumentById",
                    routeValues: new
                    {
                        internalId = created.Value
                    },
                    value: new DocumentPostCreatedResponse
                    {
                        InternalId = created.Value
                    }
                );

                await transactions.SaveTransaction(request.Id, result, cancellationToken);

                return result;
            },
            async error => TypedResults.Problem(
                detail: error.Value,
                statusCode: StatusCodes.Status500InternalServerError
            )
        );
    }

    public async Task<IResult> UpdateDocumentAsync(string internalId, DocumentPutRequest request, CancellationToken cancellationToken)
    {
        var transaction = await transactions.GetTransaction(request.Id, cancellationToken);

        if (transaction is not null) { return transaction; }

        var getResult = await documentStorage.GetDocumentByIdAsync(internalId, cancellationToken);

        return await getResult.Match<Task<IResult>>(
            async document => {
                var updateResult = await documentStorage.UpdateDocumentAsync(internalId, request.Data, cancellationToken);

                return await updateResult.Match<Task<IResult>>(
                    async success => {
                        var result = TypedResults.Ok();

                        await transactions.SaveTransaction(request.Id, result, cancellationToken);

                        return result;
                    },
                    async error => TypedResults.Problem(
                        detail: error.Value,
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                );
            },
            async notfound => {
                var result =  TypedResults.NotFound();

                await transactions.SaveTransaction(internalId, result, cancellationToken);

                return result;
            });
    }

    public async Task<IResult> DeleteDocumentAsync(string internalId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
