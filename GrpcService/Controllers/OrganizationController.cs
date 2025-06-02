using Grpc.Core;
using GrpcService.Services;
using GrpcService.Services.Organization;
using GrpcService.Services.Organization.Models;
using static Grpc.Core.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GrpcService.Controllers
{
    public class OrganizationController : OrganizationServices.OrganizationServicesBase
    {
        private readonly IOrganizationService organizationService;
        private readonly ILogger<OrganizationController> logger;

        public OrganizationController(IOrganizationService organizationService, ILogger<OrganizationController> logger)
        {
            this.organizationService = organizationService;
            this.logger = logger;
        }

        public override Task<CreateOrganizationResponse> CreateOrganization(CreateOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var command = new CreateOrganizationCommand
                {
                    Name = request.Name,
                    Address = request.Address
                };

                var result = organizationService.CreateOrganization(command);
                var response = new CreateOrganizationResponse
                {
                    OrganizationId = result.Value
                };
                logger.LogInformation("Organization created successfully. Id: {Id}, Name: {Name}", response.OrganizationId, command.Name);
                return Task.FromResult(response);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while creating organization.");
                throw;
            }
        }

        public override Task<GetOrganizationResponse> GetOrganization(GetOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var result = organizationService.GetOrganization(request.Id);

                var val = result.Value;
                var response = new GetOrganizationResponse
                {
                    Name = val.Name,
                    Address = val.Address,
                    CreatedAt = val.CreatedAt,
                    UpdatedAt = val.UpdatedAt
                };
                logger.LogInformation("Organization details: {@Response}", response);
                return Task.FromResult(response);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while fetching the organization details.");
                throw;
            }
        }

        public override Task<QueryOrganizationsResponse> QueryOrganizations(QueryOrganizationsRequest request, ServerCallContext context)
        {
            try
            {
                var queryCommand = new QueryOrganizationCommand
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    OrderBy = request.OrderBy,
                    Direction = request.Direction,
                    QueryString = request.QueryString
                };

                var queryResult = organizationService.QueryOrganizations(queryCommand);
                if (queryResult.Value == null)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "No data found."));
                }
                var result = new QueryOrganizationsResponse
                {
                    Page = queryResult.Value.Pagination.Page,
                    PageSize = queryResult.Value.Pagination.PageSize,
                    Total = queryResult.Value.Pagination.Total,
                    OrganizationList =
                    {
                        queryResult.Value.Result.Select(p => new OrganizationList
                        {
                            Id = p.Id,
                            Name=p.Name,
                            Address = p.Address,
                            CreatedAt=p.CreatedAt
                        })
                    }
                };
                logger.LogInformation("Organization list retrieved successfully.");
                return Task.FromResult(result);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while fetching the organization list.");
                throw;
            }
        }

        public override Task<UpdateOrganizationResponse> UpdateOrganization(UpdateOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var command = new UpdateOrganizationCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    Address = request.Address
                };
                var result = organizationService.UpdateOrganization(command);
                logger.LogInformation("Organization updated successfully for Id: {Id}", command.Id);
                return Task.FromResult(new UpdateOrganizationResponse());
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while updating the organization.");
                throw;
            }
        }

        public override Task<DeleteOrganizationResponse> DeleteOrganization(DeleteOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var result = organizationService.DeleteOrganization(request.Id);
                logger.LogInformation("Organization deleted successfully for Id: {Id}", request.Id);
                return Task.FromResult(new DeleteOrganizationResponse());
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred while deleting the organization.");
                throw;
            }
        }
    }
}
