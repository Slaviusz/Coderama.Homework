namespace Coderama.Homework.Contracts.Validators;

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
