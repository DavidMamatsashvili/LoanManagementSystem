using LoanManagementSystem.Models;

namespace LoanManagementSystem.DTOs
{
    public class LoanDetailsDto
    {
        public int LoanId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerPersonalNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths {  get; set; }
        public decimal MonthlyPayment {  get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PaymentResponseDto> Payments { get; set; }
        public List<LoanScheduleDto> LoanSchedules { get; set; }
        public LoanDetailsDto(int loanId, int customerId, string? customerFirstName, string? customerLastName, string? customerPersonalNumber, decimal amount, decimal interestRate, int termMonths, decimal monthlyPayment, string? status, DateTime createdAt, List<PaymentResponseDto> payments, List<LoanScheduleDto> loanScheduleDtos)
        {
            LoanId = loanId;
            CustomerId = customerId;
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerPersonalNumber = customerPersonalNumber;
            Amount = amount;
            InterestRate = interestRate;
            TermMonths = termMonths;
            MonthlyPayment = monthlyPayment;
            Status = status;
            CreatedAt = createdAt;
            Payments = payments;
            LoanSchedules = loanScheduleDtos;
        }
        public LoanDetailsDto() { }
    }
}
