namespace Coderama.Api.Validators;

using Abstractions.Contracts.Requests;

public class DocumentPutRequestValidator : AbstractValidator<DocumentPutRequest> {
    public DocumentPutRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(request => request.Data.ToString())
            .NotEmpty()
            .MinimumLength(2);
    }
}
