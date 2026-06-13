using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Services.Abstractions
{
    public interface ICustomerService
    {
        Task<Result<CustomerLoginAndRegisterResponseDto>> RegisterCustomer(CustomerRegisterRequestDto customerdto);
        Task<Result<CustomerLoginAndRegisterResponseDto>> LoginCustomer(CustomerLoginRequestDto dto);
        Task<CustomerDto> GetCustomerById(int id);
        Task<List<LoanDetailsDto>> GetLoansByCustomerid(int id);
    }
}
