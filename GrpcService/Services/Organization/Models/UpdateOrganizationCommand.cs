namespace GrpcService.Services.Organization.Models
{
    public class UpdateOrganizationCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
