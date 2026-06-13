using LoanManagementSystem.Models;

namespace LoanManagementSystem.Services.Abstractions
{
    public interface IJwtService
    {
        string GenerateToken(int customerid, string customeremail, Roles role);
    }
}
