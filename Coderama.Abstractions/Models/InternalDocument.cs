namespace Coderama.Abstractions.Models;

public sealed record InternalDocument(
    string InternalId,
    IEnumerable<string> Tags,
    JsonDocument Data
);
