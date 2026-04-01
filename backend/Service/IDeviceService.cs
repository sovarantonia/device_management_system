using backend.Entity;

namespace backend.Service
{
    public interface IDeviceService
    {
        public Task SaveAsync(DeviceRequest deviceToSave);
        public Task<bool> DeleteAsync(Guid id);
        public Task<Device?> GetByIdAsync(Guid id);
        public Task<List<Device>> GetAllAsync();
        public Task<bool> UpdateDetailsAsync(Guid id, DeviceRequest device);
        public Task<List<Device>> GetUserDevicesAsync(Guid userId);
    }
}
