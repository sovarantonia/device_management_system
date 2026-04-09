using backend.Entity;
using backend.Entity.DTO;
using Microsoft.AspNetCore.Identity;

namespace backend.Service
{
    public interface IUserService
    {
        public Task<IdentityResult> DeleteAsync(string id);
        public List<User> GetAll();
        public Task<IdentityResult> RegisterAsync(UserRequest request);
        public Task<User> GetByIdAsync(string id);
        public Task<User> UpdateAsync(string id, UserRequest user);
        public Task<User> FindByEmailAsync(string email);
    }
}
