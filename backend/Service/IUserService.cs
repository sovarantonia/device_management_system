using backend.Entity;
using backend.Entity.DTO;

namespace backend.Service
{
    public interface IUserService
    {
        public Task<User> SaveAsync(UserRequest userToSave);
        public Task DeleteAsync(Guid id);
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(Guid id);
        public Task<bool> ExistsByEmailAsync(string email);
        public Task<User> UpdateAsync(Guid id, UserRequest request);
    }
}
