using Azure.Core;
using backend.Entity;
using backend.Entity.DTO;
using backend.Entity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace backend.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> manager)
        {
            this.userManager = manager;
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            return await userManager.DeleteAsync(user);
        }

        public List<User> GetAll()
        {
            return userManager.Users.ToList();
        }

        public async Task<IdentityResult> RegisterAsync(UserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                UserName = request.Email,
                Role = request.Role,
                Location = request.Location,
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                {
                    throw new ResourceConflictException($"Email {user.Email} already exists.");
                }

                throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with id {id} does not exist");
            }

            return user;
        }

        public async Task<User> UpdateAsync(string id, UserRequest request)
        {
            var user = await GetByIdAsync(id);

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                {
                    throw new ResourceConflictException($"Email {user.Email} already exists.");
                }

                throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return user;
        }
    }
}
