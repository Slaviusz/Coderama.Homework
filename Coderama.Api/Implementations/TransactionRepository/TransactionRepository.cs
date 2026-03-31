namespace Coderama.Api.Implementations.TransactionRepository;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// In-memory transactions repository to implement idempotency
/// </summary>
[SuppressMessage("ReSharper", "AsyncMethodWithoutAwait")]
public sealed class TransactionRepository : ITransactionRepository {

    private readonly ILogger<TransactionRepository> _logger;
    private readonly Dictionary<string, IResult> _transactions = [];

    public TransactionRepository(ILogger<TransactionRepository> logger)
    {
        _logger = logger;
    }

    public async Task<IResult?> GetTransaction(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Getting transaction {id}");
        return _transactions.GetValueOrDefault(id);
    }

    public async Task SaveTransaction(string id, IResult transaction, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Saving transaction {id}");
        _transactions[id] = transaction;
    }
}
