using Grpc.Core;
using GrpcService.Common;
using GrpcService.Data;
using GrpcService.Data.Entity;
using GrpcService.Data.Repositories.Organization;
using GrpcService.Migrations;
using GrpcService.Services.Organization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace GrpcService.Services.Organization
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            this.organizationRepository = organizationRepository;
        }

        public Result<int> CreateOrganization(CreateOrganizationCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Name is required."));
            }

            if (organizationRepository.GetOrganizationByName(command.Name))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Organization Name already exists."));
            }
            var entity = organizationRepository.CreateOrganization(command);
            return new Result<int>(entity);
        }

        public Result<GetOrganizationResult> GetOrganization(int id)
        {
            if (id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Id is required."));
            }

            var organizationUser = organizationRepository.GetOrganizationById(id);
            if (organizationUser == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Organization does not exist or is inactive."));
            }

            return new Result<GetOrganizationResult>(organizationUser);
        }

        public Result<QueryOrganizationsResult> QueryOrganizations(QueryOrganizationCommand parameters)
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
            return new Result<QueryOrganizationsResult>(organizationRepository.QueryOrganizations(parameters));
        }

        public EmptyResult UpdateOrganization(UpdateOrganizationCommand command)
        {
            if (command.Id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Id is required."));
            }
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Name is required."));
            }

            //Check if organization name exists before updating.
            if (organizationRepository.GetOrganizationByNameAndOrgID(command))
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Organization Name already exists."));
            }

            organizationRepository.UpdateOrganization(command);
            return new EmptyResult();
        }

        public EmptyResult DeleteOrganization(int id)
        {
            if (id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Organization Id is required."));
            }

            organizationRepository.DeleteOrganization(id);
            return new EmptyResult();
        }
    }
}
