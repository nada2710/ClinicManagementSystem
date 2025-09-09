using ClinicApplication.Services.Account;
using ClinicApplication.Services.VerifyEmail;
using ClinicApplication.ServicesAbstractions.Account;
using ClinicApplication.ServicesAbstractions.VerifyEmail;
using ClinicApplication.Validation.Account;
using ClinicDomain.AppSettings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            // Register application services
            services.RegisterApplicationServices(configuration);
            services.OptionsSetup(configuration);
            return services;
        }
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IEmailHandler, EmailHandler>();
            services.AddMemoryCache();
            return services;
        }
        private static void OptionsSetup(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure your application settings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        }
    }
}
