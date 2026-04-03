namespace Coderama.Api.Implementations.DocumentStorage;

using Abstractions.Models;
using ZiggyCreatures.Caching.Fusion;

internal class FileStorage : IDocumentStorage {

    private readonly IFusionCache _cache;

    public FileStorage(IFusionCache cache)
    {
        _cache = cache;
    }

    public async ValueTask<OneOf<Success<InternalDocument>, NotFound>> GetDocumentByIdAsync(string internalId, CancellationToken cancellationToken)
    {
        var document = await _cache.GetOrSetAsync<InternalDocument>(
            internalId,
            async _ => await LoadDocumentFromDisk(internalId, cancellationToken),
            TimeSpan.FromMinutes(60),
            cancellationToken
        );

        return new Success<InternalDocument>(document);
    }

    public async Task<OneOf<Success<string>, Error<string>>> StoreDocumentAsync(string documentId, JsonDocument content, CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);
        var internalId = Guid.NewGuid();
        return new Success<string>(internalId.ToString("D"));
    }

    public async Task<OneOf<Success, Error<string>>> UpdateDocumentAsync(string internalId, JsonDocument content, CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);

        await _cache.RemoveAsync(internalId, token: cancellationToken); // Remove cached item when updated

        return new Success();
    }

    private static readonly string[] AttrNames = ["name", "path", "user", "comment", "source", "type"];
    private static readonly char[] Charset = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    internal static async Task<InternalDocument> LoadDocumentFromDisk(string internalId, CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);
        return await Task.FromResult(
            new InternalDocument(
                internalId,
                [],
                JsonDocument.Parse(
                    $$"""
                      {
                        "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}",
                        "{{AttrNames[Random.Shared.Next(AttrNames.Length)]}}": "{{new(Random.Shared.GetItems(Charset, 8))}}"
                      }
                      """
                )
            )
        );
    }
}
