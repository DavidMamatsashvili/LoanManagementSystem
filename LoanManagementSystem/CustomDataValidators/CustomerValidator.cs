using FluentValidation;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.CustomDataValidators
{
    public class CustomerValidator : AbstractValidator<CustomerRegisterRequestDto>
    {
        public CustomerValidator()
        {
            RuleFor(e => e.BirthDate)
                .Must(x => x <= DateTime.Today.AddYears(-18))
                .WithMessage("Customer must be at least 18 years old.");
        }
    }
}
