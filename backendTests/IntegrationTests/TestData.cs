using backend.Entity;
using backend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backendTests.IntegrationTests
{
    public static class TestData
    {
        public static void InsertData(AppDbContext context)
        {
            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Guid deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            Guid deviceId2 = Guid.Parse("33333333-3333-3333-3333-333333333333");
            Guid deviceId3 = Guid.Parse("44444444-4444-4444-4444-444444444444");

            Guid userId2 = Guid.Parse("55555555-5555-5555-5555-555555555555");

            context.Users.AddRange(
            [
                new User
                    {
                        Id = userId,
                        Name = "Integration User",
                        Email = "integration@test.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("test123")
                    },
                    new User
                    {
                        Id = userId2,
                        Name = "Integration User 2",
                        Email = "integration2@test.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("test123")
                    },

                ]);

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
