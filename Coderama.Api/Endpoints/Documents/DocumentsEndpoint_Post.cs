namespace Coderama.Api.Endpoints.Documents;

using Abstractions.Contracts.Requests;
using Abstractions.Contracts.Responses;
using Abstractions.Interfaces;
using DependencyInjection;
using Validators;

file class DocumentsEndpoint {
    public static void Post(IEndpointRouteBuilder app)
    {
        app.MapPost("/documents",
            async (
                DocumentPostRequest request,
                ILogger<DocumentsEndpoint> logger,
                IDocumentStorage documentStorage,
                CancellationToken cancellationToken
            ) => {
                logger.LogInformation($"Processing POST Request");

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
            })
            .AddEndpointFilter<ValidationFilter<DocumentPostRequest>>()
            .WithName("AddDocument")
            .WithTags("Documents")
            .WithDescription("Add new document")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Produces<DocumentPostCreatedResponse>(StatusCodes.Status201Created)
            .Produces<BadRequestObjectResult>(StatusCodes.Status400BadRequest)
            ;
    }
}
