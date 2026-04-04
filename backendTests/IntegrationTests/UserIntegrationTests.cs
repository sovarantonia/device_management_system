using backend.Entity.DTO;
using backend.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace backendTests.IntegrationTests
{
    [TestClass]
    public class UserIntegrationTests
    {
        private static CustomWebApplicationFactory _factory = null!;
        private static HttpClient _client = null!;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [TestInitialize]
        public void TestInit()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Devices.RemoveRange(db.Devices);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();

            TestData.InsertData(db);
        }

        [TestMethod]
        public async Task GetUserById()
        {
            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var response = await _client.GetAsync($"/user/{userId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var user = await response.Content.ReadFromJsonAsync<UserResponse>();

            Assert.IsNotNull(user);
            Assert.AreEqual(userId, user.Id);
            Assert.AreEqual("Integration User", user.Name);
            Assert.AreEqual("integration@test.com", user.Email);
        }

        [TestMethod]
        public async Task GetUserById_UserNotFound()
        {
            var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var response = await _client.GetAsync($"/user/{missingId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual($"User with id {missingId} does not exist", error.Message);
        }

        [TestMethod]
        public async Task GetAllUsers()
        {
            var response = await _client.GetAsync("/user");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();

            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count);
            Assert.IsTrue(users.Any(u => u.Email == "integration@test.com"));
            Assert.IsTrue(users.Any(u => u.Email == "integration2@test.com"));
        }

        [TestMethod]
        public async Task SaveUser()
        {
            var request = new UserRequest
            {
                Name = "Charlie",
                Role = "User",
                Location = "Somewhere",
                Email = "charlie@test.com",
                Password = "secret123"
            };

            var response = await _client.PostAsJsonAsync("/user", request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var user = await response.Content.ReadFromJsonAsync<UserResponse>();

            Assert.IsNotNull(user);
            Assert.AreEqual("Charlie", user.Name);
            Assert.AreEqual("charlie@test.com", user.Email);
            Assert.AreNotEqual(Guid.Empty, user.Id);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var savedUser = await db.Users.FindAsync(user.Id);

            Assert.IsNotNull(savedUser);
            Assert.AreEqual("Charlie", savedUser.Name);
            Assert.AreEqual("charlie@test.com", savedUser.Email);
        }

        [TestMethod]
        public async Task SaveUser_EmailAlreadyExists()
        {
            var request = new UserRequest
            {
                Name = "Another",
                Role = "User",
                Location = "somewhere",
                Email = "integration@test.com",
                Password = "secret123"
            };

            var response = await _client.PostAsJsonAsync("/user", request);

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual("User with email integration@test.com already exists", error.Message);
        }

        [TestMethod]
        public async Task DeleteUser()
        {
            Guid userId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var response = await _client.DeleteAsync($"/user/{userId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var deletedUser = await db.Users.FindAsync(userId);

            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public async Task DeleteUser_NotFound()
        {
            var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var response = await _client.DeleteAsync($"/user/{missingId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual($"User with id {missingId} does not exist", error.Message);
        }

        [TestMethod]
        public async Task UpdateUser()
        {
            var request = new UserRequest
            {
                Name = "Alice",
                Location = "Far far away"
            };
            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var response = await _client.PutAsJsonAsync($"/user/{userId}", request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var user = await response.Content.ReadFromJsonAsync<UserResponse>();

            Assert.IsNotNull(user);
            Assert.AreEqual(userId, user.Id);
            Assert.AreEqual("Alice", user.Name);
            Assert.AreEqual("Far far away", user.Location);
            Assert.AreEqual("integration@test.com", user.Email);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var updatedUser = await db.Users.FindAsync(userId);

            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Alice", updatedUser.Name);
            Assert.AreEqual("Far far away", updatedUser.Location);
            Assert.AreEqual("integration@test.com", updatedUser.Email);
        }

        [TestMethod]
        public async Task UpdateUser_NotFound()
        {
            var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var request = new UserRequest
            {
                Name = "Updated Name"
            };

            var response = await _client.PutAsJsonAsync($"/user/{missingId}", request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual($"User with id {missingId} does not exist", error.Message);
        }

        [TestMethod]
        public async Task UpdateUser_EmailAlreadyExists()
        {
            var request = new UserRequest
            {
                Email = "integration2@test.com"
            };

            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var response = await _client.PutAsJsonAsync($"/user/{userId}", request);

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual("User with email integration2@test.com already exists", error.Message);
        }
    }
}
