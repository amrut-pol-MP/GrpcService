namespace GrpcService.Data.Entity
{
    public class UserOrganizationAssociationEntity
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
