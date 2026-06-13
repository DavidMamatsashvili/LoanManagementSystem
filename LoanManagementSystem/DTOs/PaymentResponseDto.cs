namespace LoanManagementSystem.DTOs
{
    public class PaymentResponseDto
    {
        public int CustomerId { get; set; }
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentResponseDto(int id, int loanId, decimal amount, DateTime paymentdate)
        {
            CustomerId = id;
            LoanId = loanId;
            Amount = amount;
            PaymentDate = paymentdate;
        }
        public PaymentResponseDto() { }
    }
}
