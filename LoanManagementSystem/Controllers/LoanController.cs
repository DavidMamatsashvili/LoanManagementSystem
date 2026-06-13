using System.Security.Claims;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService loanService;
        public LoanController(ILoanService loanservice)
        {
            this.loanService = loanservice;
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpPost("CreateApplication")]
        public async Task<IActionResult> CreateLoanApplication([FromBody] LoanRequestDto loandto)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            LoanRequestDto? createdLoanDto = await loanService.CreateLoanApplication(id,loandto);

            if (createdLoanDto is null)
            {
                return BadRequest("Failed to create loan application.");
            }

            return StatusCode(StatusCodes.Status201Created, createdLoanDto);
            //return CreatedAtAction(
            //    nameof(GetLoans),
            //    createdLoanDto);
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            LoanDetailsDto? loan = await loanService.GetLoanById(id);
            if (loan is null)
            {
                return NotFound($"Loan with Id {id} was not found.");
            }
            return Ok(loan);
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpGet("GetLoanByCustomerId")]
        public async Task<IActionResult> GetLoanByCustomerId()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var loans = await loanService.GetLoansByCustomerId(id);
            if (loans == null || !loans.Any())
            {
                return NotFound("No loans found for this customer.");
            }
            return Ok(loans);
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpGet("GetLoanScheduleByLoanId/{id}")]
        public async Task<IActionResult> GetLoanSchedulesByLoanId(int id)
        {
            var schedules = await loanService.GetLoanSchedulesByLoanId(id);
            if (schedules == null || !schedules.Any())
            {
                return NotFound($"No payment schedule found for loan with Id {id}.");
            }
            return Ok(schedules);
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpGet("GetLoanStatusByLoanId/{loanId}")]
        public async Task<IActionResult> GetLoanStatus(int loanId)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            string? email = User.FindFirstValue(ClaimTypes.Email);

            var result = await loanService.GetLoanStatus(loanId, id, email);
            return Ok(result);
        }
    }
}
