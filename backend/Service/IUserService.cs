using backend.Entity;

namespace backend.Service
{
    public interface IUserService
    {
        public Task SaveAsync(User userToSave);
        public Task DeleteAsync(Guid id);
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(Guid id);
    }
}
