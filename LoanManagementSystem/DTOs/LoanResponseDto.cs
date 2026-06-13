using LoanManagementSystem.Models;

namespace LoanManagementSystem.DTOs
{
    public class LoanResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public LoanResponseDto(int id, int customerId, decimal amount, decimal interestRate, int termMonths, decimal monthlyPayment, LoanStatus status, DateTime createdAt)
        {
            Id = id;
            CustomerId = customerId;
            Amount = amount;
            InterestRate = interestRate;
            TermMonths = termMonths;
            MonthlyPayment = monthlyPayment;
            Status = status;
            CreatedAt = createdAt;
        }
        public LoanResponseDto() { }
    }
}
