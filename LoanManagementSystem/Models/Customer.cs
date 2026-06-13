namespace LoanManagementSystem.Models
{
    //add email and password
    public class Customer
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PersonalNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int CreditScore { get; set; }
        public Roles Role {  get; set; } 
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
