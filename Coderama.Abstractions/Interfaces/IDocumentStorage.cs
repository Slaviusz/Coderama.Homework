namespace Coderama.Abstractions.Interfaces;

public interface IDocumentStorage {
    Task<OneOf<Success<InternalDocument>, NotFound>> GetDocumentByIdAsync(string internalId, CancellationToken cancellationToken);
    Task<OneOf<Success<string>, Error<string>>> StoreDocumentAsync(string documentId, JsonDocument content, CancellationToken cancellationToken);
    Task<OneOf<Success, Error<string>>> UpdateDocumentAsync(string internalId, JsonDocument content, CancellationToken cancellationToken);
}
