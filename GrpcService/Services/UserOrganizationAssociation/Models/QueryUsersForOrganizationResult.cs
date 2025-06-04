using GrpcService.Common;

namespace GrpcService.Services.UserOrganizationAssociation.Models
{
    public class QueryUsersForOrganizationResult
    {
        public Pagination Pagination { get; set; }
        public ICollection<GetUsersForOrganizationResult> Result { get; set; }
    }
}
