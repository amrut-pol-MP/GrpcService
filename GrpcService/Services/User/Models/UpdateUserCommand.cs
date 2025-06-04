namespace GrpcService.Services.User.Models
{
    public class UpdateUserCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
