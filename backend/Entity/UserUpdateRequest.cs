namespace backend.Entity
{
    public class UserUpdateRequest
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public ICollection<Device> Devices { get; set; } = new List<Device>();

        public UserUpdateRequest() { }
    }
}
