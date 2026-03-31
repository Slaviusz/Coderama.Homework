namespace Coderama.Homework.Contracts.Requests;

public record DocumentPostRequest(
    string Id,
    IEnumerable<string> Tags,
    JsonDocument Data
);
