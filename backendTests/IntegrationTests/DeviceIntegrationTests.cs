using backend.Entity;
using backend.Entity.DTO;
using backend.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace backendTests.IntegrationTests
{
    [TestClass]
    public class DeviceIntegrationTests
    {
        private static CustomWebApplicationFactory _factory = null!;
        private static HttpClient _client = null!;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _factory = new CustomWebApplicationFactory();
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
            var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Devices.RemoveRange(db.Devices);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();

            TestData.InsertData(db);
        }

        private HttpClient CreateAuthenticatedClient(
            string userId = "11111111-1111-1111-1111-111111111111",
            string email = "integration@test.com",
            string role = "User")
        {
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("X-Test-UserId", userId);
            client.DefaultRequestHeaders.Add("X-Test-Email", email);
            client.DefaultRequestHeaders.Add("X-Test-Role", role);

            return client;
        }

        private HttpClient CreateUnauthenticatedClient()
        {
            return _factory.CreateClient();
        }


        [TestMethod]
        public async Task GetDevices()
        {
            _client = CreateAuthenticatedClient();

            var response = await _client.GetAsync("/device");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var devices = await response.Content.ReadFromJsonAsync<List<DeviceResponse>>();

            Assert.AreEqual(4, devices.Count);

            Assert.IsTrue(devices.Any(d => d.Name == "Phone1"));
            Assert.IsTrue(devices.Any(d => d.Name == "Tablet1"));
            Assert.IsTrue(devices.Any(d => d.Name == "Tablet left alone"));
            Assert.IsTrue(devices.Any(d => d.Name == "Tablet owned"));
        }

        [TestMethod]
        public async Task GetDevices_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            var response = await _client.GetAsync("/device");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetUserDevices()
        {
            _client = CreateAuthenticatedClient();

            var userId = "11111111-1111-1111-1111-111111111111";
            var response = await _client.GetAsync($"/device/user/{userId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var devices = await response.Content.ReadFromJsonAsync<List<DeviceResponse>>();

            Assert.AreEqual(2, devices.Count);

            Assert.IsTrue(devices.Any(d => d.Name == "Phone1"));
            Assert.IsTrue(devices.Any(d => d.Name == "Tablet1"));
        }

        [TestMethod]
        public async Task GetUserDevices_UserNotFound()
        {
            _client = CreateAuthenticatedClient();

            var userId = "11111111-1111-1111-1111-111111111112";
            var response = await _client.GetAsync($"/device/user/{userId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();

            Assert.IsNotNull(error);
            Assert.IsFalse(string.IsNullOrEmpty(error.Message));

            Assert.AreEqual($"User with id {userId} not found", error.Message);
        }

        [TestMethod]
        public async Task GetUserDevices_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            var userId = "11111111-1111-1111-1111-111111111112";
            var response = await _client.GetAsync($"/device/user/{userId}");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetDeviceById()
        {
            _client = CreateAuthenticatedClient();

            var deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var response = await _client.GetAsync($"/device/{deviceId1}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();
            Assert.AreEqual("Phone1", device.Name);
            Assert.AreEqual("11111111-1111-1111-1111-111111111111", device.User.Id);
            Assert.AreEqual(deviceId1, device.Id);
            Assert.AreEqual(DeviceType.Phone.ToString(), device.DeviceType);
        }

        [TestMethod]
        public async Task GetDeviceById_NotFound()
        {
            _client = CreateAuthenticatedClient();

            Guid deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var response = await _client.GetAsync($"/device/{deviceId1}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();
            Assert.AreEqual($"Device with id {deviceId1} not found", error.Message);
        }

        [TestMethod]
        public async Task GetDeviceById_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            Guid deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var response = await _client.GetAsync($"/device/{deviceId1}");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteDevice()
        {
            _client = CreateAuthenticatedClient();
            Guid deviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var response = await _client.DeleteAsync($"/device/{deviceId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteDevice_NotFound()
        {
            _client = CreateAuthenticatedClient();
            Guid deviceId = Guid.Parse("44444444-4444-4444-4444-444444444445");
            var response = await _client.DeleteAsync($"/device/{deviceId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();
            Assert.AreEqual($"Device with id {deviceId} not found", error.Message);
        }

        [TestMethod]
        public async Task DeleteDevice_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();
            Guid deviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var response = await _client.DeleteAsync($"/device/{deviceId}");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task SaveDevice()
        {
            _client = CreateAuthenticatedClient();

            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                DeviceType = DeviceType.Phone.ToString(),
                Description = "test device",
                OS = "test os",
                OSVersion = "3.2",
                RamAmount = 16,
                Manufacturer = "tester",
                Processor = "i3",
            };

            var response = await _client.PostAsJsonAsync("/device", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var deviceResponse = await response.Content.ReadFromJsonAsync<DeviceResponse>();
            Assert.IsNotNull(deviceResponse);
            Assert.AreEqual("test device", deviceResponse.Name);
            Assert.AreEqual("test device", deviceResponse.Description);
        }

        [TestMethod]
        public async Task SaveDevice_InvalidType()
        {
            _client = CreateAuthenticatedClient();

            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                DeviceType = "type",
                Description = "test device",
                OS = "test os",
                OSVersion = "3.2",
                RamAmount = 16,
                Manufacturer = "tester",
                Processor = "i3",
            };

            var response = await _client.PostAsJsonAsync<DeviceRequest>("/device", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();
            Assert.AreEqual("Invalid device type. Should be Phone or Tablet", error.Message);
        }

        [TestMethod]
        public async Task SaveDevice_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                DeviceType = "type",
                Description = "test device",
                OS = "test os",
                OSVersion = "3.2",
                RamAmount = 16,
                Manufacturer = "tester",
                Processor = "i3",
            };

            var response = await _client.PostAsJsonAsync("/device", request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateDeviceDetails()
        {
            _client = CreateAuthenticatedClient();

            Guid deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                Description = "test device",
                DeviceType = "Phone"
            };

            var response = await _client.PutAsJsonAsync($"/device/{deviceId}", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();
            Assert.AreEqual("test device", device.Name);
            Assert.AreEqual("test device", device.Description);
            Assert.AreEqual(DeviceType.Phone.ToString(), device.DeviceType);
        }

        [TestMethod]
        public async Task UpdateDeviceDetails_NotFound()
        {
            _client = CreateAuthenticatedClient();

            Guid deviceId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                Description = "test device",
                DeviceType = "Phone",
            };

            var response = await _client.PutAsJsonAsync($"/device/{deviceId}", request);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();
            Assert.AreEqual($"Device with id {deviceId} not found", error.Message);
        }

        [TestMethod]
        public async Task UpdateDeviceDetails_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            Guid deviceId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                Description = "test device",
            };

            var response = await _client.PutAsJsonAsync($"/device/{deviceId}", request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task AssignDevice()
        {
            _client = CreateAuthenticatedClient();

            var deviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var response = await _client.PutAsync($"/device/{deviceId}/assign", null);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();

            Assert.IsNotNull(device);
            Assert.AreEqual(deviceId, device.Id);
        }

        [TestMethod]
        public async Task AssignDevice_Conflict()
        {
            _client = CreateAuthenticatedClient();

            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222"); ;


            var response = await _client.PutAsync($"/device/{deviceId}/assign", null);

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual("Device is already assigned to a user", error.Message);
        }

        [TestMethod]
        public async Task AssignDevice_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222"); ;

            var response = await _client.PutAsync($"/device/{deviceId}/assign", null);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task UnassignDevice()
        {
            _client = CreateAuthenticatedClient();

            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var response = await _client.PutAsync($"/device/{deviceId}/unassign", null);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();

            Assert.IsNotNull(device);
            Assert.AreEqual(deviceId, device.Id);
        }

        [TestMethod]
        public async Task UnassignDevice_AnotherUserDevice()
        {
            _client = CreateAuthenticatedClient();

            var deviceId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            var response = await _client.PutAsync($"/device/{deviceId}/unassign", null);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<MessageResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual("Cannot unassign a device you do not own", error.Message);
        }

        [TestMethod]
        public async Task UnassignDevice_NotAuthenticated()
        {
            _client = CreateUnauthenticatedClient();

            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var response = await _client.PutAsync($"/device/{deviceId}/unassign", null);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
