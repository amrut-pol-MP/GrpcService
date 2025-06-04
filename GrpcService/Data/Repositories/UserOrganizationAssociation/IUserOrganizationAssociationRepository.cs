using GrpcService.Services.User.Models;
using GrpcService.Services.UserOrganizationAssociation.Models;

namespace GrpcService.Data.Repositories.UserOrganizationAssociation
{
    public interface IUserOrganizationAssociationRepository
    {
        int AssociateUserToOrganization(AssociateUserToOrganizationCommand command);
        bool ExistUserByOrganizationId(AssociateUserToOrganizationCommand command);
        void DisassociateUserFromOrganization(DisassociateUserFromOrganizationCommand command);
        QueryUsersForOrganizationResult QueryUsersForOrganization(QueryUsersForOrganizationCommand criteria);
    }
}
