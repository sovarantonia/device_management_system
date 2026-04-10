namespace backend.Entity.DTO
{
    public class UserLoginResponse
    {
        public UserResponse? User { get; set; }
        public string? Token { get; set; }
    }
}
