using System.Reflection.Metadata.Ecma335;
using Azure;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Migrations;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;

namespace LoanManagementSystem.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly LoanManagementContext context;
        private readonly IJwtService jwtService;
        public CustomerService(LoanManagementContext context, IJwtService jwtService)
        {
            this.context = context;
            this.jwtService = jwtService;
        }

        public async Task<Result<CustomerLoginAndRegisterResponseDto>> RegisterCustomer(CustomerRegisterRequestDto customerdto)
        {
            Customer? existingCustomer = await context.Customers
                .FirstOrDefaultAsync(e => e.Email == customerdto.Email);

            if (existingCustomer is not null)
            {
                return Result<CustomerLoginAndRegisterResponseDto>.Failure("The user with this email already exists.");
            }

            string hash = BCrypt.Net.BCrypt.HashPassword(customerdto.Password);

            Customer customer = new Customer
            {
                Email = customerdto.Email,
                PasswordHash = hash,
                FirstName = customerdto.FirstName,
                LastName = customerdto.LastName,
                PersonalNumber = customerdto.PersonalNumber,
                BirthDate = customerdto.BirthDate,
                CreditScore = 300,
                Role = Roles.Customer,
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var customerLogin = await LoginCustomer(new CustomerLoginRequestDto(customerdto.Email,customerdto.Password));
            int customerId = customerLogin.CustomerId;
            string email = customerLogin.CustomerEmail!;
            string token = jwtService.GenerateToken(customerId, email!, Roles.Customer);

            CustomerLoginAndRegisterResponseDto response = new CustomerLoginAndRegisterResponseDto(
                customerId,
                customer.FirstName,
                customer.LastName,
                customer.PersonalNumber,
                customer.BirthDate,
                customer.Email,
                token
                );

            return Result<CustomerLoginAndRegisterResponseDto>.Success(response,customerId,email);
        }

        public async Task<Result<CustomerLoginAndRegisterResponseDto>> LoginCustomer(CustomerLoginRequestDto dto)
        {
            Customer? customer = await context.Customers.FirstOrDefaultAsync(e => e.Email == dto.Email);

            if (customer is null)
            {
                return Result<CustomerLoginAndRegisterResponseDto>.Failure("The user with this email doesnt exist");
            }

            bool verify = BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash);

            if (!verify)
            {
                return Result<CustomerLoginAndRegisterResponseDto>.Failure("wrong email or password");
            }

            string token = jwtService.GenerateToken(customer.Id, customer.Email, Roles.Customer);

            CustomerLoginAndRegisterResponseDto result = new CustomerLoginAndRegisterResponseDto(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.PersonalNumber,
                customer.BirthDate,
                customer.Email,
                token
                );

            return Result<CustomerLoginAndRegisterResponseDto>.Success(result,customer.Id,customer.Email);
        }

        public async Task<CustomerDto> GetCustomerById(int id)
        {
            Customer? customer = await context.Customers
                .Include(c => c.Loans)
                    .ThenInclude(l => l.Payments)
                .Include(c => c.Loans)
                    .ThenInclude(l => l.LoanSchedules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer is null)
            {
                return null;
            }

            return new CustomerDto()
            {
                Id = customer.Id,
                Email = customer.Email,
                PasswordHash = customer.PasswordHash,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PersonalNumber = customer.PersonalNumber,
                BirthDate = customer.BirthDate,
                CreditScore = customer.CreditScore,
                LoanDetails = customer.Loans.Select(l => new LoanDetailsDto()
                {
                    LoanId = l.Id,
                    CustomerId = customer.Id,
                    CustomerFirstName = customer.FirstName,
                    CustomerLastName = customer.LastName,
                    CustomerPersonalNumber = customer.PersonalNumber,
                    Amount = l.Amount,
                    InterestRate = l.InterestRate,
                    TermMonths = l.TermMonths,
                    MonthlyPayment = l.MonthlyPayment,
                    Status = l.Status.ToString(),
                    CreatedAt = l.CreatedAt,
                    Payments = l.Payments.Select(p => new PaymentResponseDto()
                    {
                        CustomerId = customer.Id,
                        LoanId = l.Id,
                        Amount = p.Amount,
                        PaymentDate = p.PaymentDate,
                    }).ToList(),
                    LoanSchedules = l.LoanSchedules.Select(s=>new LoanScheduleDto()
                    {
                        Id = s.Id,
                        LoanId = s.LoanId,
                        PMT = s.PMT,
                        Date = s.Date,
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<List<LoanDetailsDto>> GetLoansByCustomerid(int id)
        {
            List<LoanDetailsDto> loans = await context.Loans.Where(e=>e.CustomerId==id).
                Select(e=>new LoanDetailsDto(
                    e.Id,
                    e.CustomerId,
                    e.Customer.FirstName,
                    e.Customer.LastName,
                    e.Customer.PersonalNumber,
                    e.Amount,
                    e.InterestRate,
                    e.TermMonths,
                    e.MonthlyPayment,
                    e.Status.ToString(),
                    e.CreatedAt,
                    e.Payments.Select(k=>new PaymentResponseDto(
                        k.Id,
                        k.LoanId,
                        k.Amount,
                        DateTime.UtcNow
                        )).ToList(),
                    e.LoanSchedules.Select(k=>new LoanScheduleDto(
                        k.Id,
                        k.LoanId,
                        k.PMT,
                        k.Date
                        )).ToList()
                    )).ToListAsync();
            return loans;
        }
    }
}
