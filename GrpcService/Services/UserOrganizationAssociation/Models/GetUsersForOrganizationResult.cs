namespace GrpcService.Services.UserOrganizationAssociation.Models
{
    public class GetUsersForOrganizationResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long CreatedAt { get; set; }
    }
}
