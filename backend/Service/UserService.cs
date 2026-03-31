
using backend.Entity;
using backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext dbContext;

        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveAsync(User userToSave)
        {
            dbContext.Users.Add(userToSave);
            await dbContext.SaveChangesAsync();
        }
    }
}
