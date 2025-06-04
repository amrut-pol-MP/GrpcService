namespace GrpcService.Services.User.Models
{
    public class GetUserResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
