using GrpcService.Common;

namespace GrpcService.Services.User.Models
{
    public class QueryUsersResult
    {
        public Pagination Pagination { get; set; }
        public ICollection<GetUserResult> Result { get; set; }
    }
}
