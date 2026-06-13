using System.Security.Claims;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository.Data;
using LoanManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        public PaymentController(IPaymentService service)
        {
            this.paymentService = service;
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpPost("payments")]
        public async Task<IActionResult> CreatePaymentPost([FromBody] PaymentRequestDto paymentdto)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Result<PaymentRequestDto> payment = await paymentService.MakePaymentAsync(id,paymentdto);
            if(!payment.IsSuccess)
            {
                return BadRequest("Payment could not be processed.");
            }
            return Ok(payment);
        }


        [Authorize(Roles = nameof(Roles.Customer))]
        [HttpGet("GetPaymentsByLoanId/{id}")]
        public async Task<IActionResult> GetPaymentsByLoanId(int id)
        {
            List<PaymentResponseDto> result = await paymentService.GetPaymentsByLoanId(id);
            return Ok(result);
        }
    }
}
