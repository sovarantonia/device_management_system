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
        public async Task GetDevices()
        {
            var response = await _client.GetAsync("/device");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var devices = await response.Content.ReadFromJsonAsync<List<DeviceResponse>>();

            Assert.AreEqual(3, devices.Count);

            Assert.IsTrue(devices.Any(d => d.Name == "Phone1"));
            Assert.IsTrue(devices.Any(d => d.Name == "Tablet1"));
            Assert.IsTrue(devices.Any(d => d.Name == "Tablet left alone"));
        }

        [TestMethod]
        public async Task GetUserDevices()
        {
            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
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
            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111112");
            var response = await _client.GetAsync($"/device/user/{userId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.IsFalse(string.IsNullOrEmpty(error.Message));

            Assert.AreEqual($"User with id {userId} not found", error.Message);
        }

        [TestMethod]
        public async Task GetDeviceById()
        {
            Guid deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var response = await _client.GetAsync($"/device/{deviceId1}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();
            Assert.AreEqual("Phone1", device.Name);
            Assert.AreEqual(Guid.Parse("11111111-1111-1111-1111-111111111111"), device.UserId);
            Assert.AreEqual(deviceId1, device.Id);
            Assert.AreEqual(DeviceType.Phone.ToString(), device.DeviceType);
        }

        [TestMethod]
        public async Task GetDeviceById_NotFound()
        {
            Guid deviceId1 = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var response = await _client.GetAsync($"/device/{deviceId1}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.AreEqual($"Device with id {deviceId1} not found", error.Message);
        }

        [TestMethod]
        public async Task DeleteDevice()
        {
            Guid deviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var response = await _client.DeleteAsync($"/device/{deviceId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteDevice_NotFound()
        {
            Guid deviceId = Guid.Parse("44444444-4444-4444-4444-444444444445");
            var response = await _client.DeleteAsync($"/device/{deviceId}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.AreEqual($"Device with id {deviceId} not found", error.Message);
        }

        [TestMethod]
        public async Task SaveDevice()
        {
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

            var response = await _client.PostAsJsonAsync<DeviceRequest>("/device", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var deviceResponse = await response.Content.ReadFromJsonAsync<DeviceResponse>();
            Assert.IsNotNull(deviceResponse);
            Assert.AreEqual("test device", deviceResponse.Name);
            Assert.AreEqual("test device", deviceResponse.Description);
            Assert.AreEqual(null, deviceResponse.UserId);
        }

        [TestMethod]
        public async Task SaveDevice_InvalidType()
        {
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

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.AreEqual("Invalid device type. Should be Phone or Tablet", error.Message);      
        }

        [TestMethod]
        public async Task UpdateDeviceDetails()
        {
            Guid deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                Description = "test device",
            };

            var response = await _client.PutAsJsonAsync<DeviceRequest>($"/device/{deviceId}", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();
            Assert.AreEqual("test device", device.Name);
            Assert.AreEqual("test device", device.Description);
            Assert.AreEqual(DeviceType.Phone.ToString(), device.DeviceType);
        }

        [TestMethod]
        public async Task UpdateDeviceDetails_NotFound()
        {
            Guid deviceId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            DeviceRequest request = new DeviceRequest
            {
                Name = "test device",
                Description = "test device",
            };

            var response = await _client.PutAsJsonAsync<DeviceRequest>($"/device/{deviceId}", request);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.AreEqual($"Device with id {deviceId} not found", error.Message);
        }

        [TestMethod]
        public async Task AssignDevice()
        {
            var deviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var response = await _client.PutAsync($"/device/{deviceId}/assign?userId={userId}", null);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();

            Assert.IsNotNull(device);
            Assert.AreEqual(deviceId, device.Id);
            Assert.AreEqual(userId, device.UserId);
        }

        [TestMethod]
        public async Task AssignDevice_UserNotFound()
        {
            var deviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var missingUserId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var response = await _client.PutAsync($"/device/{deviceId}/assign?userId={missingUserId}", null);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual($"User with id {missingUserId} not found", error.Message);
        }

        [TestMethod]
        public async Task AssignDevice_Conflict()
        {
            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222"); ;
            var userId = Guid.Parse("55555555-5555-5555-5555-555555555555"); ;

            var response = await _client.PutAsync($"/device/{deviceId}/assign?userId={userId}", null);

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual("Device is already assigned to a user", error.Message);
        }

        [TestMethod]
        public async Task UnassignDevice()
        {
            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var response = await _client.PutAsync($"/device/{deviceId}/unassign?userId={userId}", null);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var device = await response.Content.ReadFromJsonAsync<DeviceResponse>();

            Assert.IsNotNull(device);
            Assert.AreEqual(deviceId, device.Id);
            Assert.IsNull(device.UserId);
        }

        [TestMethod]
        public async Task UnassignDevice_UserNotFound()
        {
            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var missingUserId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var response = await _client.PutAsync($"/device/{deviceId}/unassign?userId={missingUserId}", null);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual($"User with id {missingUserId} not found", error.Message);
        }

        [TestMethod]
        public async Task UnassignDevice_AnotherUserDevice()
        {
            var deviceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var userId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            var response = await _client.PutAsync($"/device/{deviceId}/unassign?userId={userId}", null);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.IsNotNull(error);
            Assert.AreEqual("Cannot unassign a device you do not own", error.Message);
        }
    }
}
