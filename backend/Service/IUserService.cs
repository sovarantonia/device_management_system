using backend.Entity;
using backend.Entity.DTO;
using Microsoft.AspNetCore.Identity;

namespace backend.Service
{
    public interface IUserService
    {
        public Task<IdentityResult> DeleteAsync(string id);
        public List<AppUser> GetAll();
        public Task<IdentityResult> RegisterAsync(UserRequest request);
        public Task<AppUser> GetByIdAsync(string id);
        public Task<AppUser> UpdateAsync(string id, UserRequest user);
        public Task<AppUser> FindByEmailAsync(string email);
    }
}
