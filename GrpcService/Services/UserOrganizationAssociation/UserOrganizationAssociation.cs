using Grpc.Core;
using GrpcService.Common;
using GrpcService.Data.Repositories.Organization;
using GrpcService.Data.Repositories.User;
using GrpcService.Data.Repositories.UserOrganizationAssociation;
using GrpcService.Services.UserOrganizationAssociation.Models;
using Microsoft.AspNetCore.Mvc;


namespace GrpcService.Services.UserOrganizationAssociation
{
    public class UserOrganizationAssociation : IUserOrganizationAssociation
    {
        private readonly IUserOrganizationAssociationRepository userOrganizationAssociationRepository;
        private readonly IUserRepository userRepository;
        private readonly IOrganizationRepository organizationRepository;

        public UserOrganizationAssociation(IUserOrganizationAssociationRepository userOrganizationAssociationRepository, IUserRepository userRepository, IOrganizationRepository organizationRepository)
        {
            this.userOrganizationAssociationRepository = userOrganizationAssociationRepository;
            this.userRepository = userRepository;
            this.organizationRepository = organizationRepository;
        }

        public Result<int> AssociateUserToOrganization(AssociateUserToOrganizationCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Id is required."));
            }            
            if (command.UserId <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id is required."));
            }

            //Check if the user exists or is inactive before association.
            var userDetails = userRepository.GetUserById(command.UserId);
            if (userDetails == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User does not exist or is inactive."));
            }

            //Check if the organization exists or is inactive before association.
            var organizationUserDetails = organizationRepository.GetOrganizationById(command.OrganizationId);
            if (organizationUserDetails == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Organization does not exist or is inactive."));
            }

            if (userOrganizationAssociationRepository.ExistUserByOrganizationId(command))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User - Organization association already exists."));
            }

            var entity = userOrganizationAssociationRepository.AssociateUserToOrganization(command);
            return new Result<int>(entity);
        }

        public EmptyResult DisassociateUserFromOrganization(DisassociateUserFromOrganizationCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Id is required."));
            }
            if (command.UserId <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id is required."));
            }

            userOrganizationAssociationRepository.DisassociateUserFromOrganization(command);
            return new EmptyResult();
        }

        public Result<QueryUsersForOrganizationResult> QueryUsersForOrganization(QueryUsersForOrganizationCommand parameters)
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
            return new Result<QueryUsersForOrganizationResult>(userOrganizationAssociationRepository.QueryUsersForOrganization(parameters));
        }
    }
}
