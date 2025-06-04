namespace GrpcService.Services.User.Models
{
    public class CreateUserCommand
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
