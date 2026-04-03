namespace backend.Entity.DTO
{
    public class UserResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public List<DeviceSummary> Devices { get; set; } = new();
    }
}
