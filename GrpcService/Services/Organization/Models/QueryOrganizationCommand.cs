namespace GrpcService.Services.Organization.Models
{
    public class QueryOrganizationCommand
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? Direction { get; set; }
        public string? QueryString { get; set; }
    }
}
