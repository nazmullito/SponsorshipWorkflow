using FluentValidation;
using Sponsorship.Application.DTOs.Requests;

namespace Sponsorship.Application.Validators
{
    public class CreateSponsorshipRequestValidator
    : AbstractValidator<CreateSponsorshipRequestDto>
    {
        public CreateSponsorshipRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.RequestorName)
                .NotEmpty();

            RuleFor(x => x.Department)
                .NotEmpty();

            RuleFor(x => x.EventName)
                .NotEmpty();

            RuleFor(x => x.Purpose)
                .NotEmpty();

            RuleFor(x => x.RequestedAmount)
                .GreaterThan(0);

            RuleFor(x => x.EventDate)
                .GreaterThan(DateTime.UtcNow.Date);
        }
    }
}
