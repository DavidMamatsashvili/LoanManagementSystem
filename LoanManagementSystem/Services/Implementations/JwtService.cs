using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace LoanManagementSystem.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerateToken(int customerid, string customeremail, Roles role)
        {
            var Issuer = configuration["Jwt:Issuer"];
            var Audience = configuration["Jwt:Audience"];
            var Key = configuration["Jwt:Key"];

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key!));

            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:Issuer,
                audience:Audience,
                signingCredentials:Credentials,
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,customerid.ToString()),
                    new Claim(ClaimTypes.Email,customeremail),
                    new Claim(ClaimTypes.Role,role.ToString())
                },
                expires: DateTime.UtcNow.AddMinutes(30)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
