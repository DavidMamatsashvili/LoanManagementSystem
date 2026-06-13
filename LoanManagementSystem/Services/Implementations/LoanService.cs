using System.Reflection.Metadata.Ecma335;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly LoanManagementContext context;
        public LoanService(LoanManagementContext context)
        {
            this.context = context;
        }

        public async Task<LoanRequestDto?> CreateLoanApplication(int customerid, LoanRequestDto dto)
        {
            var customer = await context.Customers.FindAsync(customerid);

            if(customer == null)
            {
                return null;
            }

            var loan = new Loan
            {
                Amount = dto.Amount,
                TermMonths = dto.TermMonths,
                CustomerId = customerid,
                InterestRate = dto.InterestRate,
                CreatedAt = DateTime.UtcNow,
                Status = LoanStatus.Pending,
            };

            int customerAge = DateTime.Now.Year - customer.BirthDate.Year;

            if (
                loan.Amount < 500 || loan.Amount > 50000 ||
                loan.TermMonths < 6 || loan.TermMonths > 60 ||
                customerAge < 18 ||
                customer.CreditScore < 300
            )
            {
                loan.Status = LoanStatus.Rejected;
            }
            else
            {
                loan.Status = LoanStatus.Approved;
            }

            decimal monthlyPayment;

            if (loan.InterestRate == 0)
            {
                monthlyPayment = decimal.Round(
                    loan.Amount / loan.TermMonths,
                    2,
                    MidpointRounding.AwayFromZero);
            }
            else
            {
                double monthlyRate = (double)loan.InterestRate / 100 / 12;

                monthlyPayment = (decimal)((double)loan.Amount *
                    (monthlyRate * Math.Pow(1 + monthlyRate, loan.TermMonths)) /
                    (Math.Pow(1 + monthlyRate, loan.TermMonths) - 1));

                monthlyPayment = decimal.Round(monthlyPayment, 2);
            }

            loan.MonthlyPayment = monthlyPayment;

            await context.Loans.AddAsync(loan);
            await context.SaveChangesAsync();

            if (loan.Status == LoanStatus.Approved)
            {
                var startDate = DateTime.UtcNow;

                var schedules = new List<LoanSchedule>();

                for (int i = 0; i < loan.TermMonths; i++)
                {
                    schedules.Add(new LoanSchedule
                    {
                        LoanId = loan.Id,
                        PMT = monthlyPayment,
                        Date = startDate.AddMonths(i + 1)
                    });
                }

                await context.LoanSchedules.AddRangeAsync(schedules);
                await context.SaveChangesAsync();
            }

            return dto;
        }

        public async Task<LoanDetailsDto> GetLoanById(int loanid)
        {
            Loan? loan = await context.Loans
                .Include(e => e.Customer)
                .Include(e => e.Payments)
                .Include(e => e.LoanSchedules)
                .FirstOrDefaultAsync(e => e.Id == loanid);

            if(loan == null)
            {
                return null;
            }

            List<PaymentResponseDto> payments = new List<PaymentResponseDto>();
            List<LoanScheduleDto> loanSchedules = new List<LoanScheduleDto>();

            foreach (Payment payment in loan.Payments)
            {
                payments.Add(new PaymentResponseDto(payment.Id, payment.LoanId, payment.Amount,DateTime.UtcNow));
            }
            foreach (LoanSchedule schedule in loan.LoanSchedules)
            {
                loanSchedules.Add(new LoanScheduleDto(schedule.Id, schedule.LoanId, schedule.PMT, schedule.Date));
            }

            LoanDetailsDto details = new LoanDetailsDto(
                loan.Id,
                loan.CustomerId,
                loan.Customer.FirstName,
                loan.Customer.LastName,
                loan.Customer.PersonalNumber,
                loan.Amount,
                loan.InterestRate,
                loan.TermMonths,
                loan.MonthlyPayment,
                loan.Status.ToString(),
                loan.CreatedAt,
                payments,
                loanSchedules
                );

            return details;
        }

        public async Task<List<LoanDetailsDto>> GetLoansByCustomerId(int customerid)
        {
            List<Loan> loans = await context.Loans.Include(e => e.Customer)
                    .Include(e => e.Payments)
                    .Include(e => e.LoanSchedules).Where(e => e.CustomerId == customerid).ToListAsync();

            if(loans is null)
            {
                return null;
            }

            List<LoanDetailsDto> details = new List<LoanDetailsDto>();

            foreach(Loan loan in loans)
            {
                List<PaymentResponseDto> payments = new List<PaymentResponseDto>();
                List<LoanScheduleDto> loanSchedules = new List<LoanScheduleDto>();

                foreach (Payment payment in loan.Payments)
                {
                    payments.Add(new PaymentResponseDto(payment.Id, payment.LoanId, payment.Amount, DateTime.UtcNow));
                }
                foreach (LoanSchedule schedule in loan.LoanSchedules)
                {
                    loanSchedules.Add(new LoanScheduleDto(schedule.Id, schedule.LoanId, schedule.PMT, schedule.Date));
                }

                LoanDetailsDto detail = new LoanDetailsDto(
                    loan.Id,
                    loan.CustomerId,
                    loan.Customer.FirstName,
                    loan.Customer.LastName,
                    loan.Customer.PersonalNumber,
                    loan.Amount,
                    loan.InterestRate,
                    loan.TermMonths,
                    loan.MonthlyPayment,
                    loan.Status.ToString(),
                    loan.CreatedAt,
                    payments,
                    loanSchedules
                    );

                details.Add(detail);
            }

            return details;
        }

        public async Task<List<LoanScheduleDto>> GetLoanSchedulesByLoanId(int loanid)
        {
            Loan? loan = await context.Loans.Include(e => e.LoanSchedules).Where(e => e.Id == loanid).SingleOrDefaultAsync();

            if(loan is null)
            {
                return null;
            }

            List<LoanScheduleDto> schedules = loan.LoanSchedules.Select(e =>
                new LoanScheduleDto(e.Id, e.LoanId, e.PMT, e.Date)
            ).ToList();

            return schedules;
        }

        public async Task<Result<string>> GetLoanStatus(int loanid, int customerid, string email)
        {
            Loan? loan = await context.Loans.Where(e=>e.Id == loanid).SingleOrDefaultAsync();

            if(loan is null)
            {
                return null;
            }

            return Result<string>.Success(loan.Status.ToString(), customerid, email);
        }
    }
}
