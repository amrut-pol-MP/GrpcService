namespace GrpcService.Services.UserOrganizationAssociation.Models
{
    public class DisassociateUserFromOrganizationCommand
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
    }
}
