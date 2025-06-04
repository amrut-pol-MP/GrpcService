using GrpcService.Common;
using GrpcService.Services.Organization.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService.Services.Organization
{
    public interface IOrganizationService
    {
        Result<int> CreateOrganization(CreateOrganizationCommand command);
        Result<GetOrganizationResult> GetOrganization(int id);
        Result<QueryOrganizationsResult> QueryOrganizations(QueryOrganizationCommand parameters);
        EmptyResult UpdateOrganization(UpdateOrganizationCommand command);
        EmptyResult DeleteOrganization(int id);
    }
}
