using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly LoanManagementContext context;
        public PaymentService(LoanManagementContext context)
        {
            this.context = context;
        }

        public async Task<Result<PaymentRequestDto>> MakePaymentAsync(int customerId, PaymentRequestDto dto)
        {
            var loan = await context.Loans
                .Include(l => l.Customer)
                .Include(l => l.LoanSchedules)
                .FirstOrDefaultAsync(l => l.Id == dto.LoanId && l.CustomerId == customerId);

            if (loan is null)
            {
                return Result<PaymentRequestDto>.Failure("Loan does not exist.");
            }

            if (loan.Status == LoanStatus.Closed)
            {
                return Result<PaymentRequestDto>.Failure("You cannot pay a closed loan.");
            }

            if (dto.Amount <= 0)
            {
                return Result<PaymentRequestDto>.Failure("Payment amount must be greater than 0.");
            }

            if (dto.Amount > loan.Amount)
            {
                return Result<PaymentRequestDto>.Failure("Payment amount cannot exceed the remaining loan balance.");
            }

            var payment = new Payment
            {
                LoanId = loan.Id,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow
            };

            context.Payments.Add(payment);

            LoanSchedule? schedule = loan.LoanSchedules
                .FirstOrDefault(s => s.Date.Year == payment.PaymentDate.Year && s.Date.Month == payment.PaymentDate.Month);

            if (schedule != null && payment.PaymentDate.Date > schedule.Date.Date)
            {
                loan.Customer.CreditScore -= 50;
            }

            loan.Amount -= dto.Amount;

            if (loan.Amount <= 0)
            {
                loan.Amount = 0;
                loan.Status = LoanStatus.Closed;
            }
            else if (dto.Amount >= loan.MonthlyPayment)
            {
                loan.Customer.CreditScore += 50;
            }
            else
            {
                loan.Customer.CreditScore -= 50;
            }

            await context.SaveChangesAsync();

            return Result<PaymentRequestDto>.Success(dto, loan.CustomerId, loan.Customer.Email);
        }

        public async Task<List<PaymentResponseDto>> GetPaymentsByLoanId(int loanid)
        {
            List<Payment> res = await context.Payments
                .Include(e => e.Loan)
                    .ThenInclude(e => e.Customer)
                .Where(e => e.LoanId == loanid)
                .ToListAsync();

            List<PaymentResponseDto> payments = new List<PaymentResponseDto>();
            foreach (var payment in res)
            {
                PaymentResponseDto pay = new PaymentResponseDto(payment.Loan.CustomerId,payment.LoanId,payment.Amount,payment.PaymentDate);
                payments.Add(pay);
            }
            return payments;
        }
    }
}
