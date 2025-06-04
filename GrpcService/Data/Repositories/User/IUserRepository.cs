using GrpcService.Data.Entity;
using GrpcService.Services.User.Models;

namespace GrpcService.Data.Repositories.User
{
    public interface IUserRepository
    {
        int CreateUser(CreateUserCommand command);
        bool GetUserByEmail(string email);
        bool GetUserByUserName(string userName);
        GetUserResult GetUserById(int id);
        QueryUsersResult QueryUsers(QueryUserCommand criteria);
        void UpdateUser(UpdateUserCommand command);
        void DeleteUser(int id);
        bool GetUserByEmailAndUserId(UpdateUserCommand command);
        bool GetUserByUserNameAndUserId(UpdateUserCommand command);
    }
}
