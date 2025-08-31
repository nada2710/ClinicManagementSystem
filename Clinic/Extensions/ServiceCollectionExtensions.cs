using ClinicInfrastructure.Extensions;

namespace ClinicAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Infrastructure services
            services.AddInfrastructureServices(configuration);
            return services;
        }
    }
}
