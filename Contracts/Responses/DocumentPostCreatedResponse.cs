namespace Coderama.Homework.Contracts.Responses;

public record DocumentPostCreatedResponse {
    public required string InternalId { get; init; }
}
