using System.Runtime.InteropServices;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly LoanManagementContext context;
        private readonly IJwtService jwtService;
        public AdminService(LoanManagementContext context, IJwtService service)
        {
            this.context = context;
            this.jwtService = service;
        }

        public async Task<Result<AdminResponseDto>> AdminLogin(AdminRequestDto adminRequest)
        {
            Admin? admin = await context.Admins.Where(a => a.Email == adminRequest.Email).FirstOrDefaultAsync();

            if (admin is null)
            {
                return null;
            }

            bool verify = BCrypt.Net.BCrypt.Verify(adminRequest.Password, admin.PasswordHash);
            if(!verify)
            {
                return Result<AdminResponseDto>.Failure("wrong email or password");
            }

            string token = jwtService.GenerateToken(admin.Id, admin.Email, Roles.Admin);
            AdminResponseDto adminResponse = new AdminResponseDto(admin.Id, admin.Email, admin.PasswordHash, token);
            return Result<AdminResponseDto>.Success(adminResponse, admin.Id, admin.Email);
        }

        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            List<CustomerDto> result = await context.Customers
                .Include(c => c.Loans)
                    .ThenInclude(l => l.Payments)
                .Include(c => c.Loans)
                    .ThenInclude(l => l.LoanSchedules)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Email = c.Email,
                    PasswordHash = c.PasswordHash,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PersonalNumber = c.PersonalNumber,
                    BirthDate = c.BirthDate,
                    CreditScore = c.CreditScore,

                    LoanDetails = c.Loans.Select(l => new LoanDetailsDto
                    {
                        LoanId = l.Id,
                        CustomerId = l.CustomerId,
                        CustomerFirstName = c.FirstName,
                        CustomerLastName = c.LastName,
                        CustomerPersonalNumber = c.PersonalNumber,
                        Amount = l.Amount,
                        InterestRate = l.InterestRate,
                        TermMonths = l.TermMonths,
                        MonthlyPayment = l.MonthlyPayment,
                        Status = l.Status.ToString(),
                        CreatedAt = l.CreatedAt,

                        Payments = l.Payments.Select(p => new PaymentResponseDto
                        {
                            CustomerId = c.Id,
                            LoanId = p.LoanId,
                            Amount = p.Amount,
                            PaymentDate = p.PaymentDate
                        }).ToList(),

                        LoanSchedules = l.LoanSchedules.Select(s => new LoanScheduleDto
                        {
                            Id = s.Id,
                            LoanId = s.LoanId,
                            PMT = s.PMT,
                            Date = s.Date
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return result;
        }

        public async Task<bool> DeleteCustomerById(int userid)
        {
            Customer? customer = await context.Customers.FirstOrDefaultAsync(e => e.Id == userid);
            if (customer is null)
            {
                return false;
            }

            context.Customers.Remove(customer);
            context.SaveChanges();
            return true;
        }

        public async Task<List<LoanDetailsDto>> GetLoans()
        {
            List<Loan> loans = await context.Loans
                .Include(e => e.Customer)
                .Include(e => e.Payments)
                .Include(e => e.LoanSchedules)
                .ToListAsync();

            List<LoanDetailsDto> loanDetails = new List<LoanDetailsDto>();
            foreach (var loan in loans)
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
                loanDetails.Add(details);
            }

            return loanDetails;
        }

        public async Task<bool> DeleteLoanByLoanId(int loanid)
        {
            Loan? loan = await context.Loans.Where(e=>e.Id==loanid).FirstOrDefaultAsync();
            if(loan is null)
            {
                return false;
            }
            context.Loans.Remove(loan);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
