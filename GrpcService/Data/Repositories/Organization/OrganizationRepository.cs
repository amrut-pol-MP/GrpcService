using Grpc.Core;
using GrpcService.Common;
using GrpcService.Data.Entity;
using GrpcService.Migrations;
using GrpcService.Services.Organization.Models;
using System.Linq;

namespace GrpcService.Data.Repositories.Organization
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly GrpcServiceDbContext context;
        public OrganizationRepository(GrpcServiceDbContext context)
        {
            this.context = context;
        }
        
        public int CreateOrganization(CreateOrganizationCommand command)
        {
            var entity = new OrganizationEntity
            {
                Name = command.Name,
                Address = command.Address,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            context.Organizations.Add(entity);
            context.SaveChanges();
            return entity.Id;
        }
        
        public bool GetOrganizationByName(string name)
        {
            var entity = context.Organizations.FirstOrDefault(s => s.Name == name);
            return true ? entity != null : false;
        }
        
        public GetOrganizationResult GetOrganizationById(int id)
        {
            var entity = context.Organizations.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
            if (entity == null)
                return null;

            return new GetOrganizationResult
            {
                Name = entity.Name,
                Address = entity.Address,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
        
        public IQueryable<OrganizationEntity> GetOrganizationsList()
        {
            return this.context.Organizations.Where(p => p.DeletedAt == null);
        }
        
        public QueryOrganizationsResult QueryOrganizations(QueryOrganizationCommand parameters)
        {
            var query = GetOrganizationsList();

            if (!string.IsNullOrWhiteSpace(parameters.QueryString))
            {
                query = query.Where(s => s.Name.Contains(parameters.QueryString) || s.Address.Contains(parameters.QueryString));
            }

            var page = parameters.Page ?? 1;
            var pageSize = parameters.PageSize ?? 50;

            var orderBy = parameters.OrderBy?.ToLower() ?? "createdat";
            var direction = parameters.Direction?.ToLower() ?? "asc";

            query = (orderBy, direction) switch
            {
                ("name", "asc") => query.OrderBy(o => o.Name),
                ("name", "desc") => query.OrderByDescending(o => o.Name),
                ("address", "asc") => query.OrderBy(o => o.Address),
                ("address", "desc") => query.OrderByDescending(o => o.Address),
                ("createdat", "desc") => query.OrderByDescending(o => o.CreatedAt),
                _ => query.OrderBy(o => o.CreatedAt)
            };

            var organizationsList = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new GetOrganizationResult
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                }).ToList();

            var result = new QueryOrganizationsResult
            {
                Pagination = new Pagination(query.Count(), page, pageSize),
                Result = organizationsList
            };

            return result;
        }

        public void UpdateOrganization(UpdateOrganizationCommand command)
        {
            var entity = context.Organizations.FirstOrDefault(o => o.Id == command.Id && o.DeletedAt == null);
            if (entity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Organization does not exist or is inactive."));
            }
            entity.Name = command.Name;
            entity.Address = command.Address;
            entity.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            context.SaveChanges();
        }

        public void DeleteOrganization(int id)
        {
            var entity = context.Organizations.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
            if (entity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Organization does not exist or is inactive."));
            }
            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entity.IsDeleted = true;
            context.SaveChanges();
        }

        public bool GetOrganizationByNameAndOrgID(UpdateOrganizationCommand command)
        {
            var entity = context.Organizations.FirstOrDefault(s => s.Name == command.Name && s.Id != command.Id);
            return true ? entity != null : false;
        }
    }
}
