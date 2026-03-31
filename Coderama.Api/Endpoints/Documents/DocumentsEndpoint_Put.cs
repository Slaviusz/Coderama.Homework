namespace Coderama.Api.Endpoints.Documents;

using Abstractions.Contracts.Requests;
using Abstractions.Contracts.Responses;
using Abstractions.Interfaces;
using Validators;

file class DocumentsEndpoint {
    public static void Put(IEndpointRouteBuilder app)
    {

        app.MapPut("/documents/{internalId}",
            async (
                string internalId,
                DocumentPutRequest request,
                ILogger<DocumentsEndpoint> logger,
                IDocumentStorage documentStorage,
                CancellationToken cancellationToken
            ) => {
                logger.LogInformation($"Processing PUT Request");

                var getResult = await documentStorage.GetDocumentByIdAsync(internalId, cancellationToken);

                return await getResult.Match<Task<IResult>>(
                    async document => {
                        var updateResult = await documentStorage.UpdateDocumentAsync(document.Value.InternalId, request.Data, cancellationToken);

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
            })
            .AddEndpointFilter<ValidationFilter<DocumentPutRequest>>()
            .WithName("UpdateDocumentById")
            .WithTags("Documents")
            .WithDescription("Update existing document by its Id")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Produces<DocumentPostCreatedResponse>(StatusCodes.Status201Created)
            .Produces<BadRequestObjectResult>(StatusCodes.Status400BadRequest)
            ;
    }
}
