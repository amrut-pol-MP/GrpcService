namespace GrpcService.Services.Organization.Models
{
    public class QueryOrganizationsResult
    {
        public Pagination Pagination { get; set; }
        public ICollection<GetOrganizationResult> Result { get; set; }
    }
}
