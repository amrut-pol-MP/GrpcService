using Grpc.Core;
using GrpcService.Common;
using GrpcService.Data.Entity;
using GrpcService.Services.User.Models;

namespace GrpcService.Data.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly GrpcServiceDbContext context;

        public UserRepository(GrpcServiceDbContext context)
        {
            this.context = context;
        }

        public int CreateUser(CreateUserCommand command)
        {
            var entity = new UserEntity
            {
                Name = command.Name,
                UserName = command.UserName,
                Email = command.Email,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            context.Users.Add(entity);
            context.SaveChanges();
            return entity.Id;
        }

        public bool GetUserByEmail(string email)
        {
            var entity = context.Users.FirstOrDefault(s => s.Email == email && !s.IsDeleted);
            return true ? entity != null : false;
        }
        
        public bool GetUserByUserName(string userName)
        {
            var entity = context.Users.FirstOrDefault(s => s.UserName == userName && !s.IsDeleted);
            return true ? entity != null : false;
        }

        public GetUserResult GetUserById(int id)
        {
            var entity = context.Users.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
            if (entity == null)
                return null;

            return new GetUserResult
            {
                Name = entity.Name,
                UserName = entity.UserName,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public QueryUsersResult QueryUsers(QueryUserCommand parameters)
        {
            var query = GetUsersList();

            if (!string.IsNullOrWhiteSpace(parameters.QueryString))
            {
                query = query.Where(s => s.Name.Contains(parameters.QueryString) || s.UserName.Contains(parameters.QueryString) || s.Email.Contains(parameters.QueryString));
            }

            var page = parameters.Page ?? 1;
            var pageSize = parameters.PageSize ?? 50;

            var orderBy = parameters.OrderBy?.ToLower() ?? "createdat";
            var direction = parameters.Direction?.ToLower() ?? "asc";

            query = (orderBy, direction) switch
            {
                ("name", "asc") => query.OrderBy(o => o.Name),
                ("name", "desc") => query.OrderByDescending(o => o.Name),
                ("username", "asc") => query.OrderBy(o => o.UserName),
                ("username", "desc") => query.OrderByDescending(o => o.UserName),
                ("email", "asc") => query.OrderBy(o => o.Email),
                ("email", "desc") => query.OrderByDescending(o => o.Email),
                ("createdat", "desc") => query.OrderByDescending(o => o.CreatedAt),
                _ => query.OrderBy(o => o.CreatedAt)
            };

            var usersList = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new GetUserResult
                {
                    Id = o.Id,
                    Name = o.Name,
                    UserName = o.UserName,
                    Email = o.Email,
                    CreatedAt = o.CreatedAt
                }).ToList();

            var result = new QueryUsersResult
            {
                Pagination = new Pagination(query.Count(), page, pageSize),
                Result = usersList
            };

            return result;
        }
        
        public IQueryable<UserEntity> GetUsersList()
        {
            return this.context.Users.Where(p => p.DeletedAt == null);
        }
        
        public void UpdateUser(UpdateUserCommand command)
        {
            var entity = context.Users.FirstOrDefault(o => o.Id == command.Id && o.DeletedAt == null);
            if (entity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User does not exist or is inactive."));
            }
            entity.Name = command.Name;
            entity.UserName = command.UserName;
            entity.Email = command.Email;
            entity.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var entity = context.Users.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
            if (entity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User does not exist or is inactive."));
            }
            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entity.IsDeleted = true;
            context.SaveChanges();
        }

        public bool GetUserByEmailAndUserId(UpdateUserCommand command)
        {
            var entity = context.Users.FirstOrDefault(s => s.Email == command.Email && s.Id != command.Id && !s.IsDeleted);
            return true ? entity != null : false;
        }
        public bool GetUserByUserNameAndUserId(UpdateUserCommand command)
        {
            var entity = context.Users.FirstOrDefault(s => s.UserName == command.UserName && s.Id != command.Id && !s.IsDeleted);
            return true ? entity != null : false;
        }
    }
}
