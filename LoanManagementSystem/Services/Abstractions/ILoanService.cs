using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Services.Abstractions
{
    public interface ILoanService
    {
        Task<LoanRequestDto> CreateLoanApplication(int customerid, LoanRequestDto dto);
        Task<LoanDetailsDto> GetLoanById(int loanid);
        Task<List<LoanDetailsDto>> GetLoansByCustomerId(int customerid);
        Task<List<LoanScheduleDto>> GetLoanSchedulesByLoanId(int loanid);
        Task<Result<string>> GetLoanStatus(int loanid, int customerid, string email);
    }
}
