namespace Coderama.Api.Endpoints.Documents;

using Abstractions.Contracts.Requests;
using Abstractions.Contracts.Responses;
using DependencyInjection;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Local")]
file class DocumentsEndpoint {
    public static void Put(IEndpointRouteBuilder app)
    {

        app.MapPut("/documents/{internalId}",
            async (
                string internalId,
                DocumentPutRequest request,
                ILogger<DocumentsEndpoint> logger,
                IDocumentRepository documentRepository,
                CancellationToken cancellationToken
            ) => {
                logger.LogInformation($"Processing PUT Request");

                return await documentRepository.UpdateDocumentAsync(internalId, request, cancellationToken);
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
