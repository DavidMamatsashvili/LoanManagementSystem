using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int LoanId { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public virtual Loan Loan { get; set; } = null!;
    }
}
