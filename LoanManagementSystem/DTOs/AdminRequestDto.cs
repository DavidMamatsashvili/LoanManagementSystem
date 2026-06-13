namespace LoanManagementSystem.DTOs
{
    public class AdminRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public AdminRequestDto() { }
        public AdminRequestDto(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
