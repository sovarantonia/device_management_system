using backend.Entity;
using backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace backendTests.IntegrationTests
{
    public static class TestData
    {
        public static void InsertData(AppDbContext context)
        {
            string userId = "11111111-1111-1111-1111-111111111111";
            string userId2 = "55555555-5555-5555-5555-555555555555";

            Guid deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            Guid deviceId2 = Guid.Parse("33333333-3333-3333-3333-333333333333");
            Guid deviceId3 = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var passwordHasher = new PasswordHasher<User>();

            var user1 = new User
            {
                Id = userId,
                Name = "Integration User",
                Email = "integration@test.com",
                NormalizedEmail = "INTEGRATION@TEST.COM",
                UserName = "integration@test.com",
                NormalizedUserName = "INTEGRATION@TEST.COM",
                Role = "User",
                Location = "Test Location"
            };
            user1.PasswordHash = passwordHasher.HashPassword(user1, "test123");

            var user2 = new User
            {
                Id = userId2,
                Name = "Integration User 2",
                Email = "integration2@test.com",
                NormalizedEmail = "INTEGRATION2@TEST.COM",
                UserName = "integration2@test.com",
                NormalizedUserName = "INTEGRATION2@TEST.COM",
                Role = "User",
                Location = "Test Location 2"
            };
            user2.PasswordHash = passwordHasher.HashPassword(user2, "test123");

            context.Users.AddRange(user1, user2);

            context.Devices.AddRange(
            [
                new Device
                    {
                        Id = deviceId1,
                        Name = "Phone1",
                        UserId = userId,
                        DeviceType = DeviceType.Phone,
                    },

                    new Device
                    {
                        Id = deviceId2,
                        Name = "Tablet1",
                        UserId = userId,
                        DeviceType = DeviceType.Tablet,
                    },

                    new Device
                    {
                        Id = deviceId3,
                        Name = "Tablet left alone",
                        UserId = null,
                        DeviceType = DeviceType.Tablet,
                    },
                ]);

            context.SaveChanges();
        }
    }
}
