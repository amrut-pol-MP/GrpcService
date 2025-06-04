using GrpcService.Data.Repositories.Organization;
using GrpcService.Data.Repositories.User;
using GrpcService.Data.Repositories.UserOrganizationAssociation;
using GrpcService.Services.Organization;
using GrpcService.Services.User;
using GrpcService.Services.UserOrganizationAssociation;

namespace GrpcService.Common
{
    public class DependencyInjection
    {
        public static void Services(IServiceCollection services)
        {
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserOrganizationAssociation, UserOrganizationAssociation>();
        }

        public static void Repositories(IServiceCollection services)
        {
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserOrganizationAssociationRepository, UserOrganizationAssociationRepository>();
        }
    }
}
