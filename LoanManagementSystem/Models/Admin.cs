using System.Data;

namespace LoanManagementSystem.Models
{
    public class Admin
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public Roles Role { get; set; } = Roles.Admin;
    }
}
