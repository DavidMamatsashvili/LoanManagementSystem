using FluentValidation;
using LoanManagementSystem.DTOs;

namespace LoanManagementSystem.CustomDataValidators
{
    public class LoanValidator : AbstractValidator<LoanRequestDto>
    {
        public LoanValidator()
        {
            RuleFor(x => x.Amount)
            .InclusiveBetween(500, 50000);

            RuleFor(x => x.TermMonths)
                .InclusiveBetween(6, 60);

            //RuleFor(x => x.CustomerId)
            //    .GreaterThan(0);
        }
    }
}
