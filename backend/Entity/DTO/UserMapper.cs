namespace backend.Entity.DTO
{
    public static class UserMapper
    {
        public static UserResponse ToDTO(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Location = user.Location,
                Role = user.Role,
                Email = user.Email,
                Devices = user.Devices
                    .Select(d => new DeviceSummary
                    {
                        Id = d.Id,
                        Name = d.Name
                    }).ToList()
            };
        }
    }
}
