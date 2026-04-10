namespace backend.Entity.DTO
{
    public static class UserMapper
    {
        public static UserResponse ToDTO(AppUser user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Location = user.Location,
                Role = user.Role,
                Email = user.Email,
            };
                
        }
    }
}
