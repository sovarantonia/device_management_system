using Microsoft.AspNetCore.Identity;

namespace backend.Entity
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public ICollection<Device> Devices { get; set; } = new List<Device>();

        public User() { }
    }
}
