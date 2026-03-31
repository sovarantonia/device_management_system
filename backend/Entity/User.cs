namespace backend.Entity
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public User(Guid id, string name, string role, string location, string email, string password)
        {
            Id = id;
            Name = name;
            Role = role;
            Location = location;
            Email = email;
            Password = password;
        }

        public User(Guid id, string name, string role, string location)
        {
            Id = id;
            Name = name;
            Role = role;
            Location = location;
        }

        public User() { }

        public override string ToString()
        {
            return $"Id: {Id}, name: {Name}, role: {Role}, location: {Location}, Email: {Email}";
        }
    }
}
