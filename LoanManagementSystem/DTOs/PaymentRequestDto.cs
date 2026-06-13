using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.DTOs
{
    public class PaymentRequestDto
    {
        //public int CustomerId { get; set; }
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public PaymentRequestDto(int loanId, decimal amount)
        {
            //CustomerId = id;
            LoanId = loanId;
            Amount = amount;
        }
        public PaymentRequestDto() { }
    }
}
