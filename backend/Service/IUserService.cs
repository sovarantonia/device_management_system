using backend.Entity;

namespace backend.Service
{
    public interface IUserService
    {
        public Task<bool> SaveAsync(User userToSave);
        public Task<bool> DeleteAsync(Guid id);
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(Guid id);
        public Task<bool> ExistsByEmailAsync(string email);
        public Task<bool> UpdateAsync(Guid id, UserUpdateRequest request);
    }
}
