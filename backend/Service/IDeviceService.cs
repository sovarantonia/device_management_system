using backend.Entity;
using backend.Entity.DTO;

namespace backend.Service
{
    public interface IDeviceService
    {
        public Task<Device> SaveAsync(DeviceRequest deviceToSave);
        public Task DeleteAsync(Guid id);
        public Task<Device> GetByIdAsync(Guid id);
        public Task<List<Device>> GetAllAsync();
        public Task<Device> UpdateDetailsAsync(Guid id, DeviceRequest device);
        public Task<List<Device>> GetUserDevicesAsync(string userId);
        public Task<Device> AssignDeviceAsync(Guid deviceId, string userId);
        public Task<Device> UnassignDeviceAsync(Guid deviceId, string userId);
        public Task<List<Device>> SearchAsync(string query);
    }
}
