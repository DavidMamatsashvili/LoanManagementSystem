namespace LoanManagementSystem.DTOs
{
    public class AdminResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Token { get; set; }
        public AdminResponseDto() { }
        public AdminResponseDto(int id, string name, string passwordHash, string token)
        {
            Id = id;
            Name = name;
            PasswordHash = passwordHash;
            Token = token;
        }
    }
}
