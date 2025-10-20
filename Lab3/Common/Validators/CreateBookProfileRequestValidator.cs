using BookApi.CQRS.Requests;
using FluentValidation;

namespace BookApi.Common.Validators;

public class CreateBookProfileRequestValidator : AbstractValidator<CreateBookProfileRequest>
{
    public CreateBookProfileRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(100).WithMessage("Author name must not exceed 100 characters.");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Matches(@"^(?:\d{10}|\d{13})$").WithMessage("ISBN must be 10 or 13 digits.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid book category.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.")
            .LessThan(10000).WithMessage("Price must be less than 10,000.");

        RuleFor(x => x.PublishedDate)
            .NotEmpty().WithMessage("Published date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Published date cannot be in the future.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

        RuleFor(x => x.CoverImageUrl)
            .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.CoverImageUrl))
            .WithMessage("Cover image URL must be a valid URL.");
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

