namespace Coderama.Api.Endpoints.Documents;

using Abstractions.Contracts.Requests;
using Abstractions.Contracts.Responses;
using DependencyInjection;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Local")]
file class DocumentsEndpoint {
    public static void Post(IEndpointRouteBuilder app)
    {
        app.MapPost("/documents",
            async (
                DocumentPostRequest request,
                ILogger<DocumentsEndpoint> logger,
                IDocumentRepository documentRepository,
                CancellationToken cancellationToken
            ) => {
                logger.LogInformation($"Processing POST Request");

                return await documentRepository.StoreDocumentAsync(request, cancellationToken);
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
