namespace LoanManagementSystem.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PersonalNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int CreditScore { get; set; }
        public List<LoanDetailsDto> LoanDetails { get; set; }
        public CustomerDto(int id, string email, string passwordHash, string firstName, string lastName, string personalNumber, DateTime birthDate, int creditScore, List<LoanDetailsDto> loanDetails)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            PersonalNumber = personalNumber;
            BirthDate = birthDate;
            CreditScore = creditScore;
            LoanDetails = loanDetails;
        }
        public CustomerDto() { }
    }
}
