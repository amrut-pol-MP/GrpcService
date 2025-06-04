
using Grpc.Core;
using GrpcService.Migrations;
using GrpcService.Services.UserOrganizationAssociation;
using GrpcService.Services.UserOrganizationAssociation.Models;

namespace GrpcService.Controllers
{
    public class UserOrganizationAssociationController : UserOrganizationAssociationServices.UserOrganizationAssociationServicesBase
    {
        private readonly IUserOrganizationAssociation userOrganizationAssociationService;
        private readonly ILogger<UserOrganizationAssociationController> logger;

        public UserOrganizationAssociationController(IUserOrganizationAssociation userOrganizationAssociationService, ILogger<UserOrganizationAssociationController> logger)
        {
            this.userOrganizationAssociationService = userOrganizationAssociationService;
            this.logger = logger;
        }

        public override Task<AssociateUserToOrganizationResponse> AssociateUserToOrganization(AssociateUserToOrganizationRequest request,ServerCallContext context)
        {
            try
            {
                var command = new AssociateUserToOrganizationCommand
                {
                    OrganizationId = request.OrganizationId,
                    UserId = request.UserId
                };

                var result = userOrganizationAssociationService.AssociateUserToOrganization(command);
                var response = new AssociateUserToOrganizationResponse
                {
                    UserToOrganizationAssociationId = result.Value
                };
                logger.LogInformation("User To Organization Association created successfully. Id: {Id}", response.UserToOrganizationAssociationId);
                return Task.FromResult(response);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred during the association process.");
                throw;
            }
        }
        
        public override Task<DisassociateUserFromOrganizationResponse> DisassociateUserFromOrganization(DisassociateUserFromOrganizationRequest request,ServerCallContext context)
        {
            try
            {
                var command = new DisassociateUserFromOrganizationCommand
                {
                    OrganizationId = request.OrganizationId,
                    UserId = request.UserId
                };

                var result = userOrganizationAssociationService.DisassociateUserFromOrganization(command);

                logger.LogInformation("User from Organization Disassociated successfully");
                return Task.FromResult(new DisassociateUserFromOrganizationResponse());
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unexpected error occurred during the association process.");
                throw;
            }
        }

        public override Task<QueryUsersForOrganizationResponse> QueryUsersForOrganization(QueryUsersForOrganizationRequest request, ServerCallContext context)
        {
            try
            {
                var queryCommand = new QueryUsersForOrganizationCommand
                {
                    OrganizationId = request.OrganizationId,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    OrderBy = request.OrderBy,
                    Direction = request.Direction,
                    QueryString = request.QueryString
                };

                var queryResult = userOrganizationAssociationService.QueryUsersForOrganization(queryCommand);
                if (queryResult.Value == null)
                {
                    throw new RpcException(new Status(StatusCode.Internal, "No data found."));
                }
                var result = new QueryUsersForOrganizationResponse
                {
                    Page = queryResult.Value.Pagination.Page,
                    PageSize = queryResult.Value.Pagination.PageSize,
                    Total = queryResult.Value.Pagination.Total,
                    Users =
                    {
                        queryResult.Value.Result.Select(p => new UsersForOrganizationList
                        {
                            Id = p.Id,
                            Name=p.Name,
                            Username = p.UserName,
                            Email = p.Email,
                            CreatedAt=p.CreatedAt
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

    }
}
