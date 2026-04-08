using backend.Entity;
using Microsoft.AspNetCore.Identity;

namespace backend.Helper
{
    public static class InitializeUsers
    {
        public static async Task SetPasswordsAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var users = new[]
        {
            ("alice@example.com", "test123"),
            ("bob@example.com", "test123")
        };

            foreach (var (email, password) in users)
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
                    await userManager.UpdateAsync(user);
                }
            }
        }
    }
}
