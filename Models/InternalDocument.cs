namespace Coderama.Homework.Models;

public sealed record InternalDocument(
    string InternalId,
    IEnumerable<string> Tags,
    JsonDocument Data
);
