using GrpcService.Common;
using GrpcService.Controllers;
using GrpcService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<GrpcServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddDbContext<GrpcServiceDbContext>();

DependencyInjection.Services(builder.Services);
DependencyInjection.Repositories(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GrpcServiceDbContext>();
    dbContext.Database.Migrate();
}

app.MapGrpcService<OrganizationController>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();
