namespace Coderama.Abstractions.Interfaces;

using Microsoft.AspNetCore.Http;

public interface ITransactionRepository {
    Task<IResult?> GetTransaction(string id, CancellationToken cancellationToken);
    Task SaveTransaction(string id, IResult transaction, CancellationToken cancellationToken);
}
