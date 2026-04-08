using backend.Entity;
using backend.Entity.DTO;
using Microsoft.AspNetCore.Identity;

namespace backend.Service
{
    public interface IUserService
    {
        public Task<IdentityResult> DeleteAsync(Guid id);
        public List<User> GetAll();
        public Task<IdentityResult> RegisterAsync(UserRequest request);
        public Task<User> GetByIdAsync(Guid id);
        public Task<IdentityResult> UpdateAsync(User user);
    }
}
