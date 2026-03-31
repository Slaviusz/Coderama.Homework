namespace Coderama.Api.Endpoints.Documents;

using Abstractions.Contracts.Responses;
using Abstractions.Interfaces;
using Mappers;

file class DocumentsEndpoint {
    public static void GetById(IEndpointRouteBuilder app)
    {

        app.MapGet("/documents/{internalId}",
            async (
                string internalId,
                ILogger<DocumentsEndpoint> logger,
                IDocumentStorage documentStorage,
                [FromServices] DocumentGetByIdMapper mapper,
                CancellationToken cancellationToken
            ) => {
                logger.LogInformation($"Processing POST Request (internalId: {internalId})");

                var getResult = await documentStorage.GetDocumentByIdAsync(internalId, cancellationToken);

                return getResult.Match<IResult>(
                    success => {
                        var result = mapper.MapToResponse(success.Value);
                        return TypedResults.Ok(result);
                    },
                    notfound => TypedResults.NotFound()
                );
            })
            .WithName("GetDocumentById")
            .WithTags("Documents")
            .WithDescription("Get document by its id")
            .Produces<DocumentGetByIdResponse>()
            .Produces<NotFoundObjectResult>(StatusCodes.Status404NotFound)
            .Produces<BadRequestObjectResult>(StatusCodes.Status400BadRequest)
            ;
    }
}
