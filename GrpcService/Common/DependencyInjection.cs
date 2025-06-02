using GrpcService.Data.Repositories.Organization;
using GrpcService.Services.Organization;

namespace GrpcService.Common
{
    public class DependencyInjection
    {
        public static void Services(IServiceCollection services)
        {
            services.AddScoped<IOrganizationService, OrganizationService>();
        }

        public static void Repositories(IServiceCollection services)
        {
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();            
        }
    }
}
