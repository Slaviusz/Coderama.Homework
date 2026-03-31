namespace Coderama.Homework.Contracts.Responses;

public record DocumentGetByIdResponse {
    public required string Id { get; init; }
    public IEnumerable<string> Tags { get; init; } = [];
    public required JsonDocument Data { get; init; }
}
