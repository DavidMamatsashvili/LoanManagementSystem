using LoanManagementSystem.Models;

namespace LoanManagementSystem.DTOs
{
    public class CustomerRegisterRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PersonalNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public CustomerRegisterRequestDto() { }
        public CustomerRegisterRequestDto(string email, string password, string firstName, string lastName, string personalNumber, DateTime birthDate)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            PersonalNumber = personalNumber;
            BirthDate = birthDate;
        }
    }
}
