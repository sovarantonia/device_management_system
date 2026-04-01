using backend.Entity;
using backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class DeviceService : IDeviceService
    {
        private readonly AppDbContext dbContext;

        public DeviceService(AppDbContext context)
        {
            dbContext = context;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
            if (device == null)
            {
                return false;
            }
            dbContext.Devices.Remove(device);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Device>> GetAllAsync()
        {
            return await dbContext.Devices.ToListAsync();
        }

        public async Task<Device?> GetByIdAsync(Guid id)
        {
            return await dbContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task SaveAsync(DeviceRequest deviceToSave)
        {
            if (!Enum.TryParse<DeviceType>(deviceToSave.DeviceType, true, out var deviceType))
            {
                throw new ArgumentException("Invalid device type. Should be Phone or Tablet");
            }
            var device = new Device 
            { 
                Name = deviceToSave.Name,
                Manufacturer = deviceToSave.Manufacturer,
                DeviceType = Enum.Parse<DeviceType>(deviceToSave.DeviceType!),
                OS = deviceToSave.OS,
                OSVersion = deviceToSave.OSVersion,
                Processor = deviceToSave.Processor,
                RamAmount = deviceToSave.RamAmount ?? 0,
                Description = deviceToSave.Description,
                UserId = deviceToSave.UserId
            };

            dbContext.Devices.Add(device);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateDetailsAsync(Guid id, DeviceRequest device)
        {
            var deviceToUpdate = await dbContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
            if (deviceToUpdate == null)
            {
                return false;
            }

            if (device.Manufacturer != null)
            {
                deviceToUpdate.Manufacturer = device.Manufacturer;
            }

            if (device.OSVersion != null)
            {
                deviceToUpdate.OSVersion = device.OSVersion;
            }

            if (device.Processor != null)
            {
                deviceToUpdate.Processor = device.Processor;
            }

            if (device.Name != null)
            {
                deviceToUpdate.Name = device.Name;
            }

            if (device.Description != null)
            {
                deviceToUpdate.Description = device.Description;
            }

            if(device.RamAmount != null)
            {
                deviceToUpdate.RamAmount = device.RamAmount;
            }

            dbContext.Devices.Update(deviceToUpdate);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Device>> GetUserDevicesAsync(Guid userId)
        {
            return await dbContext.Devices.Where(d => d.UserId == userId).ToListAsync();
        }
    }
}
