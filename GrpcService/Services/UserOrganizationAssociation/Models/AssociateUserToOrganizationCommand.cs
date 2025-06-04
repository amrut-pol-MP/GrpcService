namespace GrpcService.Services.UserOrganizationAssociation.Models
{
    public class AssociateUserToOrganizationCommand
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
    }
}
