using GrpcService.Common;
using GrpcService.Data.Entity;
using GrpcService.Services.Organization.Models;

namespace GrpcService.Data.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        int CreateOrganization(CreateOrganizationCommand command);
        bool GetOrganizationByName(string name);
        GetOrganizationResult GetOrganizationById(int id);
        IQueryable<OrganizationEntity> GetOrganizationsList();
        QueryOrganizationsResult QueryOrganizations(QueryOrganizationCommand criteria);
        void UpdateOrganization(UpdateOrganizationCommand command);
        void DeleteOrganization(int id);

    }
}
