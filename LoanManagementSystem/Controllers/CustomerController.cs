using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        //CreateCustomer
        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerRegisterRequestDto customerdto)
        {
            var result = await customerService.RegisterCustomer(customerdto);
            if (result is null)
            {
                return BadRequest("Customer registration failed.");
            }
            return StatusCode(StatusCodes.Status201Created, result);
        }


        [HttpPost("LoginCustomer")]
        public async Task<IActionResult> LoginCustomer([FromBody]CustomerLoginRequestDto customerdto)
        {
            var result = await customerService.LoginCustomer(customerdto);

            if (result is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(result);
        }


        [HttpGet("GetCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            //int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var customer = await customerService.GetCustomerById(id);

            if (customer is null)
            {
                return NotFound($"Customer with Id {id} was not found.");
            }

            return Ok(customer);
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpGet("loans")]
        public async Task<IActionResult> GetLoansByCustomerId()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var loans = await customerService.GetLoansByCustomerid(id);
            return Ok(loans);
        }
    }
}
