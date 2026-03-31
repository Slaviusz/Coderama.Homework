namespace Coderama.Api.Implementations.DocumentStorage;

using Abstractions.Models;

internal class FileStorage : IDocumentStorage {

    private static readonly string[] AttrNames = ["name", "path", "creator"];
    private static readonly char[] Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    public async Task<OneOf<Success<InternalDocument>, NotFound>> GetDocumentByIdAsync(string internalId, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        var document = new InternalDocument(
            Guid.NewGuid().ToString("D"),
            [],
            JsonDocument.Parse($$"""
                               {
                                 "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}",
                                 "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}"
                               }
                               """)
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
