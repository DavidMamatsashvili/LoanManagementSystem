using LoanManagementSystem.DTOs;

namespace LoanManagementSystem.Services.Abstractions
{
    public interface IPaymentService
    {
        Task<Result<PaymentRequestDto>> MakePaymentAsync(int customerid, PaymentRequestDto dto);
        Task<List<PaymentResponseDto>> GetPaymentsByLoanId(int loanid);
    }
}
