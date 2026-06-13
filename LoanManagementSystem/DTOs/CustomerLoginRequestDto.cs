namespace LoanManagementSystem.DTOs
{
    public class CustomerLoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CustomerLoginRequestDto() { }
        public CustomerLoginRequestDto(string email, string password)
        {
            Email = email; 
            Password = password;
        }
    }
}
