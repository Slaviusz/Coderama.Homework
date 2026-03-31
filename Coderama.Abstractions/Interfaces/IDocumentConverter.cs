namespace Coderama.Abstractions.Interfaces;

public interface IDocumentConverter<TTo> where TTo : class {
    Task<TTo> ConvertAsync(InternalDocument document, CancellationToken cancellationToken);
}
