using Grpc.Core;
using GrpcService.Services.User;
using GrpcService.Services.User.Models;
namespace GrpcService.Controllers
{
    public class UserController : UserServices.UserServicesBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }
        public override Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var command = new CreateUserCommand
                {
                    Name = request.Name,
                    UserName = request.UserName,
                    Email = request.Email
                };

                var result = userService.CreateUser(command);
                var response = new CreateUserResponse
                {
                    UserId = result.Value
                };
                logger.LogInformation("User created successfully. Id: {Id}, Email: {Email}", response.UserId, command.Email);
                return Task.FromResult(response);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while creating user.");
                throw;
            }
        }
        
        public override Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                var result = userService.GetUser(request.Id);

                var val = result.Value;
                var response = new GetUserResponse
                {
                    Name = val.Name,
                    Username = val.UserName,
                    Email = val.Email,
                    CreatedAt = val.CreatedAt,
                    UpdatedAt = val.UpdatedAt
                };
                logger.LogInformation("User details: {@Response}", response);
                return Task.FromResult(response);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while fetching the User details.");
                throw;
            }
        }

        public override Task<QueryUsersResponse> QueryUsers(QueryUsersRequest request, ServerCallContext context)
        {
            try
            {
                var queryCommand = new QueryUserCommand
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    OrderBy = request.OrderBy,
                    Direction = request.Direction,
                    QueryString = request.QueryString
                };

                var queryResult = userService.QueryUsers(queryCommand);
                if (queryResult.Value == null)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "No data found."));
                }
                var result = new QueryUsersResponse
                {
                    Page = queryResult.Value.Pagination.Page,
                    PageSize = queryResult.Value.Pagination.PageSize,
                    Total = queryResult.Value.Pagination.Total,
                    Users =
                    {
                        queryResult.Value.Result.Select(p => new UsersList
                        {
                            Id = p.Id,
                            Name=p.Name,
                            Username = p.UserName,
                            Email = p.Email,
                            CreatedAt=p.CreatedAt,
                            UpdatedAt = p.UpdatedAt
                        })
                    }
                };
                logger.LogInformation("Users list retrieved successfully.");
                return Task.FromResult(result);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while fetching the users list.");
                throw;
            }
        }

        public override Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                var command = new UpdateUserCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    UserName = request.Username,
                    Email = request.Email
                };
                var result = userService.UpdateUser(command);
                logger.LogInformation("User updated successfully for Id: {Id}", command.Id);
                return Task.FromResult(new UpdateUserResponse());
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while updating the User.");
                throw;
            }
        }

        public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            try
            {
                var result = userService.DeleteUser(request.Id);
                logger.LogInformation("User deleted successfully for Id: {Id}", request.Id);
                return Task.FromResult(new DeleteUserResponse());
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while deleting the User.");
                throw;
            }
        }

    }
}
