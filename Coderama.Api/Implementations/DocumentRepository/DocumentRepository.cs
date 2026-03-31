namespace Coderama.Api.Implementations.DocumentRepository;

using Abstractions.Contracts.Requests;
using Abstractions.Contracts.Responses;
using Mappers;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DocumentRepository(IDocumentStorage documentStorage) : IDocumentRepository {


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
        var storeResult = await documentStorage.StoreDocumentAsync(request.Id, request.Data, cancellationToken);

        return storeResult.Match<IResult>(
            created => TypedResults.CreatedAtRoute(
            routeName: "GetDocumentById",
            routeValues: new { internalId = created.Value },
            value: new DocumentPostCreatedResponse { InternalId = created.Value }
        ),
        error => TypedResults.Problem(
            detail: error.Value,
            statusCode: StatusCodes.Status500InternalServerError)
        );
    }

    public async Task<IResult> UpdateDocumentAsync(string internalId, JsonDocument content, CancellationToken cancellationToken)
    {
        var getResult = await documentStorage.GetDocumentByIdAsync(internalId, cancellationToken);

        return await getResult.Match<Task<IResult>>(
            async document => {
                var updateResult = await documentStorage.UpdateDocumentAsync(document.Value.InternalId, content, cancellationToken);

                return updateResult.Match<IResult>(
                    success => TypedResults.Ok(),
                    error => TypedResults.Problem(
                        detail: error.Value,
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                );
            },
            async notfound => TypedResults.NotFound()
        );
    }

    public async Task<IResult> DeleteDocumentAsync(string internalId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
