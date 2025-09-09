using ClinicInfrastructure.DbHelper.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicInfrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices( this IServiceCollection services,IConfiguration configuration)
        {
            // Add DbContext services
            services.AddDbContextServices(configuration);
            return services;
        }
        private static void AddDbContextServices( this IServiceCollection services,IConfiguration configuration)
        {
            
            services.AddDbContext<ClinicDbContext>(options =>
                                                  options.UseSqlServer(configuration.GetConnectionString("cs")));
        }
      
    }
}
