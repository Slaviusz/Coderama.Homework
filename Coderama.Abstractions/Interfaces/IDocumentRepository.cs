namespace Coderama.Abstractions.Interfaces;

using Contracts.Requests;
using Microsoft.AspNetCore.Http;

public interface IDocumentRepository {
    Task<IResult> GetAllDocumentsAsync(CancellationToken cancellationToken);
    Task<IResult> SearchDocumentByIdAsync(string internalId, CancellationToken cancellationToken);
    Task<IResult> GetDocumentByIdAsync(string internalId, CancellationToken cancellationToken);
    Task<IResult> StoreDocumentAsync(DocumentPostRequest request, CancellationToken cancellationToken);
    Task<IResult> UpdateDocumentAsync(string internalId, JsonDocument content, CancellationToken cancellationToken);
    Task<IResult> DeleteDocumentAsync(string internalId, CancellationToken cancellationToken);
}
