using GrpcService.Common;
using GrpcService.Services.User.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService.Services.User
{
    public interface IUserService
    {
        Result<int> CreateUser(CreateUserCommand command);
        Result<GetUserResult> GetUser(int id);
        Result<QueryUsersResult> QueryUsers(QueryUserCommand parameters);
        EmptyResult UpdateUser(UpdateUserCommand command);
        EmptyResult DeleteUser(int id);
    }
}
