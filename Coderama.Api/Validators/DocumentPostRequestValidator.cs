namespace Coderama.Api.Validators;

using Abstractions.Contracts.Requests;

public class DocumentPostRequestValidator : AbstractValidator<DocumentPostRequest> {
    public DocumentPostRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(request => request.Data.ToString())
            .NotEmpty()
            .MinimumLength(2);
    }
}
