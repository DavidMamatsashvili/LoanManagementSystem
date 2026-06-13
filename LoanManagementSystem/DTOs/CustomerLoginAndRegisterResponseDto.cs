namespace LoanManagementSystem.DTOs
{
    public class CustomerLoginAndRegisterResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PersonalNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public CustomerLoginAndRegisterResponseDto() { }
        public CustomerLoginAndRegisterResponseDto(int id, string firstName, string lastName, string personalNumber, DateTime birthDate, string email, string token)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PersonalNumber = personalNumber;
            BirthDate = birthDate;
            Email = email;
            Token = token;
        }
    }
}
