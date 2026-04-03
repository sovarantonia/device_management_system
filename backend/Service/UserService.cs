
using backend.Entity;
using backend.Entity.DTO;
using backend.Entity.Exceptions;
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

        public async Task DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with id {id} does not exist");
            }

            return user;
        }

        public async Task<User> SaveAsync(UserRequest request)
        {
            var existingUser = await ExistsByEmailAsync(request.Email);
            if (existingUser)
            {
                throw new ResourceConflictException($"User with email {request.Email} already exists");
            }

            try
            {
                var userToSave = new User
                {
                    Name = request.Name,
                    Location = request.Location,
                    Role = request.Role,
                    Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                };
               
                dbContext.Users.Add(userToSave);
                await dbContext.SaveChangesAsync();

                return userToSave;
            }
            catch (DbUpdateException)
            {
                throw;
            }    
        }

        public async Task<User> UpdateAsync(Guid id, UserRequest request)
        {
            var user = await GetByIdAsync(id);


            if (request.Name != null)
            {
                user.Name = request.Name;
            }

            if (request.Email != null)
            {
                var emailExists = await ExistsByEmailAsync(request.Email);
                if (emailExists)
                {
                    throw new ResourceConflictException($"User with email {request.Email} already exists");
                }

                user.Email = request.Email;
            }

            if (request.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            if ( request.Location != null)
            {
                user.Location = request.Location;
            }

            if (request.Role != null)
            {
                user.Role = request.Role;
            }

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await dbContext.Users
                .AnyAsync(u => u.Email == email);
        }
    }
}
