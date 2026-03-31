namespace Coderama.Api.Endpoints.Documents;

using Abstractions.Contracts.Responses;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Local")]
file class DocumentsEndpoint {
    public static void GetById(IEndpointRouteBuilder app)
    {

        app.MapGet("/documents/{internalId}",
            async (
                string internalId,
                ILogger<DocumentsEndpoint> logger,
                IDocumentRepository documentRepository,
                CancellationToken cancellationToken
            ) => {
                logger.LogInformation($"Processing POST Request (internalId: {internalId})");

                return await documentRepository.GetDocumentByIdAsync(internalId, cancellationToken);
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
