namespace LoanManagementSystem.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<LoanSchedule> LoanSchedules { get; set; } = new List<LoanSchedule>();
    }
}
