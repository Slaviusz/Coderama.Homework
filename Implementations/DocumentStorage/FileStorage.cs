namespace Coderama.Homework.Implementations.DocumentStorage;

public class FileStorage : IDocumentStorage {

    public async Task<OneOf<Success<InternalDocument>, NotFound>> GetDocumentByIdAsync(string internalId, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        var document = new InternalDocument(
            Guid.NewGuid().ToString("D"),
            [],
            JsonDocument.Parse("{}")
        );
        return new Success<InternalDocument>(document);
    }

    public async Task<OneOf<Success<string>, Error<string>>> StoreDocumentAsync(string documentId, JsonDocument content, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        var internalId = Guid.NewGuid();
        return new Success<string>(internalId.ToString("D"));
    }

    public async Task<OneOf<Success, Error<string>>> UpdateDocumentAsync(string internalId, JsonDocument content, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        return new Success();
    }
}
