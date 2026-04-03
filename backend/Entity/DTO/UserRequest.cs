namespace backend.Entity.DTO
{
    public class UserRequest
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public UserRequest() { }
    }
}
