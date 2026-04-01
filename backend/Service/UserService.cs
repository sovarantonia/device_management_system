
using backend.Entity;
using backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext dbContext;

        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> SaveAsync(User userToSave)
        {
            var existingUser = await ExistsByEmailAsync(userToSave.Email);
            if (existingUser)
            {
                return false;
            }

            try
            {
                userToSave.Password = BCrypt.Net.BCrypt.HashPassword(userToSave.Password);
                dbContext.Users.Add(userToSave);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, UserUpdateRequest request)
        {
            var user = await GetByIdAsync(id);
            

            if (user == null)
            {
                return false;
            }

            if (request.Name != null)
            {
                user.Name = request.Name;
            }

            if (request.Email != null)
            {
                var emailExists = await ExistsByEmailAsync(request.Email);
                if (emailExists)
                {
                    return false;
                }

                user.Email = request.Email;
            }

            if (request.Password != null)
            {
                user.Password = request.Password;
            }

            if (!request.Devices.IsNullOrEmpty())
            {
                user.Devices = request.Devices;
            }

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await dbContext.Users
                .AnyAsync(u => u.Email == email);
        }
    }
}
