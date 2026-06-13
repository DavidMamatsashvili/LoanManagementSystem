using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> AdminLogin([FromBody]AdminRequestDto adminRequestDto)
        {
            var result = await adminService.AdminLogin(adminRequestDto);

            if (result is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await adminService.GetAllCustomers();
            return Ok(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetLoans()
        {
            var result = await adminService.GetLoans();
            return Ok(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("[action]/{loanId}")]
        public async Task<IActionResult> DeleteLoanById(int loanId)
        {
            bool deleted = await adminService.DeleteLoanByLoanId(loanId);

            if (!deleted)
            {
                return NotFound($"Loan with Id {loanId} was not found.");
            }

            return NoContent();
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCustomerById(int id)
        {
            bool deleted = await adminService.DeleteCustomerById(id);

            if (!deleted)
            {
                return NotFound($"Customer with Id {id} was not found.");
            }

            return NoContent();
        }
    }
}
