using GrpcService.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace GrpcService.Data
{
    public class GrpcServiceDbContext : DbContext
    {
        public GrpcServiceDbContext(DbContextOptions<GrpcServiceDbContext> options) : base(options) { }
        public DbSet<OrganizationEntity> Organizations { get; set; }
    }
}
