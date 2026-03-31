namespace Coderama.Homework.Contracts.Requests;

public record DocumentPutRequest(
    string Id,
    IEnumerable<string> Tags,
    JsonDocument Data
);
