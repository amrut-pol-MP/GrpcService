using GrpcService.Common;
using GrpcService.Services.UserOrganizationAssociation.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService.Services.UserOrganizationAssociation
{
    public interface IUserOrganizationAssociation
    {
        Result<int> AssociateUserToOrganization(AssociateUserToOrganizationCommand command);
        EmptyResult DisassociateUserFromOrganization(DisassociateUserFromOrganizationCommand command);
        Result<QueryUsersForOrganizationResult> QueryUsersForOrganization(QueryUsersForOrganizationCommand parameters);
    }
}
