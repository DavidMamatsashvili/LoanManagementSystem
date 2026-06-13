using FluentValidation;
using FluentValidation.AspNetCore;
using LoanManagementSystem.CustomDataValidators;
using LoanManagementSystem.Services.Abstractions;
using LoanManagementSystem.Services.Implementations;

namespace LoanManagementSystem.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureCustomServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
            services.AddValidatorsFromAssemblyContaining<LoanValidator>();
            services.AddValidatorsFromAssemblyContaining<PaymentValidator>();


            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPaymentService, PaymentService>();
            //admin email - admin@gmail.com, admin password - admin1234
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
