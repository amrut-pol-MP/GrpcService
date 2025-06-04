using Grpc.Core;
using GrpcService.Common;
using GrpcService.Data.Entity;
using GrpcService.Services.UserOrganizationAssociation.Models;

namespace GrpcService.Data.Repositories.UserOrganizationAssociation
{
    public class UserOrganizationAssociationRepository : IUserOrganizationAssociationRepository
    {
        private readonly GrpcServiceDbContext context;

        public UserOrganizationAssociationRepository(GrpcServiceDbContext context)
        {
            this.context = context;
        }
        public int AssociateUserToOrganization(AssociateUserToOrganizationCommand command)
        {
            var entity = new UserOrganizationAssociationEntity
            {
                OrganizationId = command.OrganizationId,
                UserId = command.UserId,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            context.UserOrganizationAssociations.Add(entity);
            context.SaveChanges();
            return entity.Id;
        }

        public bool ExistUserByOrganizationId(AssociateUserToOrganizationCommand command)
        {
            var entity = context.UserOrganizationAssociations.FirstOrDefault(s => s.OrganizationId == command.OrganizationId && s.UserId == command.UserId);
            return true ? entity != null : false;
        }

        public void DisassociateUserFromOrganization(DisassociateUserFromOrganizationCommand command)
        {
            var entity = context.UserOrganizationAssociations.FirstOrDefault(s => s.OrganizationId == command.OrganizationId && s.UserId == command.UserId && s.DeletedAt == null);
            if (entity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User - Organization association does not exist or is inactive."));
            }
            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entity.IsDeleted = true;

            context.SaveChanges();
        }

        public QueryUsersForOrganizationResult QueryUsersForOrganization(QueryUsersForOrganizationCommand parameters)
        {
            var query = GetUsersList(parameters.OrganizationId);

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
                .Select(o => new GetUsersForOrganizationResult
                {
                    Id = o.Id,
                    Name = o.Name,
                    UserName = o.UserName,
                    Email = o.Email,
                    CreatedAt = o.CreatedAt
                }).ToList();

            var result = new QueryUsersForOrganizationResult
            {
                Pagination = new Pagination(query.Count(), page, pageSize),
                Result = usersList
            };

            return result;
        }

        public IQueryable<GetUsersForOrganizationResult> GetUsersList(int organizationId)
        {
            var result = from user in context.Users
                         join assoc in context.UserOrganizationAssociations
                             on user.Id equals assoc.UserId
                         where user.DeletedAt == null && assoc.DeletedAt == null && assoc.OrganizationId == organizationId
                         select new GetUsersForOrganizationResult
                         {
                             Id = user.Id,
                             Name = user.Name,
                             UserName = user.UserName,
                             Email = user.Email,
                             CreatedAt = assoc.CreatedAt
                         };

            return result;
        }

    }
}
