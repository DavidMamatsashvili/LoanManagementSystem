using LoanManagementSystem.DTOs;

namespace LoanManagementSystem.Services.Abstractions
{
    public interface IAdminService
    {
        Task<Result<AdminResponseDto>> AdminLogin(AdminRequestDto adminRequest);
        Task<List<CustomerDto>> GetAllCustomers();
        Task<bool> DeleteCustomerById(int userid);
        Task<List<LoanDetailsDto>> GetLoans();
        Task<bool> DeleteLoanByLoanId(int loanid);
    }
}
