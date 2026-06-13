using FluentValidation;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.CustomDataValidators
{
    public class PaymentValidator : AbstractValidator<PaymentRequestDto>
    {
        public PaymentValidator(LoanManagementContext context)
        {
            //RuleFor(x => x.CustomerId)
            //.GreaterThan(0);

            RuleFor(x => x.LoanId)
                .GreaterThan(0);

            RuleFor(x => x.Amount)
                .GreaterThan(0);
        }
    }
}
