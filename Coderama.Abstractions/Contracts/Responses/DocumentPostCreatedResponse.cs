namespace Coderama.Abstractions.Contracts.Responses;

public record DocumentPostCreatedResponse {
    public required string InternalId { get; init; }
}
