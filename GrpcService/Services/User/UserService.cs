using Grpc.Core;
using GrpcService.Common;
using GrpcService.Data.Repositories.User;
using GrpcService.Services.User.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        public Result<int> CreateUser(CreateUserCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required."));
            }
            if (string.IsNullOrWhiteSpace(command.UserName))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Name is required."));
            }
            if (userRepository.GetUserByUserName(command.UserName))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User Name already exists."));
            }
            if (userRepository.GetUserByEmail(command.Email))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User Email already exists."));
            }
            var entity = userRepository.CreateUser(command);
            return new Result<int>(entity);
        }

        public Result<GetUserResult> GetUser(int id)
        {
            if (id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id is required."));
            }

            var userDetails = userRepository.GetUserById(id);
            if (userDetails == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User does not exist or is inactive."));
            }

            return new Result<GetUserResult>(userDetails);
        }

        public Result<QueryUsersResult> QueryUsers(QueryUserCommand parameters)
        {
            if (parameters == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "No parameters."));

            if (parameters.Page <= 0 || parameters.Page == null)
                parameters.Page = 1;

            if (parameters.PageSize == null || parameters.PageSize.Value <= 0)
                parameters.PageSize = 50;

            if (string.IsNullOrWhiteSpace(parameters.Direction))
            {
                parameters.Direction = "ASC";
            }
            return new Result<QueryUsersResult>(userRepository.QueryUsers(parameters));
        }

        public EmptyResult UpdateUser(UpdateUserCommand command)
        {
            if (command.Id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id is required."));
            }
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Name is required."));
            }
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Email is required."));
            }

            //Check if user email exists before updating.
            if (userRepository.GetUserByEmailAndUserId(command))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User Email already exists."));
            }
            //Check if user name exists before updating.
            if (userRepository.GetUserByUserNameAndUserId(command))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User Name already exists."));
            }

            userRepository.UpdateUser(command);
            return new EmptyResult();
        }

        public EmptyResult DeleteUser(int id)
        {
            if (id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id is required."));
            }

            userRepository.DeleteUser(id);
            return new EmptyResult();
        }
    }
}
