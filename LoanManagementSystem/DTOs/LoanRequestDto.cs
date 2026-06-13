using LoanManagementSystem.Models;

namespace LoanManagementSystem.DTOs
{
            // * CustomerId
            // * Amount
            // * InterestRate
            // * TermMonths
    public class LoanRequestDto
    {
        //public int LoanId { get; set; }
        //public int CustomerId { get; set; }
        public decimal Amount{ get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public LoanRequestDto(decimal amount, decimal interestRate, int termMonths)
        {
            Amount = amount;
            InterestRate = interestRate;
            TermMonths = termMonths;
        }
        public LoanRequestDto() { }
    }
}
