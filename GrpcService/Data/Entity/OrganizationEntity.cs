namespace GrpcService.Data.Entity
{
    public class OrganizationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
