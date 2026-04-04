using backend.Entity;
using backend.Entity.DTO;
using backend.Entity.Exceptions;
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
        public async Task DeleteAsync(Guid id)
        {
            var device = await GetByIdAsync(id);
            dbContext.Devices.Remove(device);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Device>> GetAllAsync()
        {
            return await dbContext.Devices.Include(d => d.User).ToListAsync();
        }

        public async Task<Device> GetByIdAsync(Guid id)
        {
            var device = await dbContext.Devices.Include(d => d.User).FirstOrDefaultAsync(d => d.Id == id);
            if (device == null)
            {
                throw new EntityNotFoundException($"Device with id {id} not found");
            }

            return device;
        }

        public async Task<Device> SaveAsync(DeviceRequest deviceRequest)
        {
            if (!Enum.TryParse<DeviceType>(deviceRequest.DeviceType, true, out var deviceType))
            {
                throw new ArgumentException("Invalid device type. Should be Phone or Tablet");
            }
            var device = new Device 
            { 
                Name = deviceRequest.Name,
                Manufacturer = deviceRequest.Manufacturer,
                DeviceType = Enum.Parse<DeviceType>(deviceRequest.DeviceType!),
                OS = deviceRequest.OS,
                OSVersion = deviceRequest.OSVersion,
                Processor = deviceRequest.Processor,
                RamAmount = deviceRequest.RamAmount ?? 0,
                Description = deviceRequest.Description,
                UserId = deviceRequest.UserId
            };

            dbContext.Devices.Add(device);
            await dbContext.SaveChangesAsync();

            return device;
        }

        public async Task<Device> UpdateDetailsAsync(Guid id, DeviceRequest deviceRequest)
        {
            var deviceToUpdate = await GetByIdAsync(id);

            if (deviceRequest.Manufacturer != null)
            {
                deviceToUpdate.Manufacturer = deviceRequest.Manufacturer;
            }

            if (deviceRequest.OSVersion != null)
            {
                deviceToUpdate.OSVersion = deviceRequest.OSVersion;
            }

            if (deviceRequest.Processor != null)
            {
                deviceToUpdate.Processor = deviceRequest.Processor;
            }

            if (deviceRequest.Name != null)
            {
                deviceToUpdate.Name = deviceRequest.Name;
            }

            if (deviceRequest.Description != null)
            {
                deviceToUpdate.Description = deviceRequest.Description;
            }

            if(deviceRequest.RamAmount != null)
            {
                deviceToUpdate.RamAmount = deviceRequest.RamAmount;
            }

            dbContext.Devices.Update(deviceToUpdate);
            await dbContext.SaveChangesAsync();

            return deviceToUpdate;
        }

        public async Task<List<Device>> GetUserDevicesAsync(Guid userId)
        {
            var userExists = await dbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userExists)
            {
                throw new EntityNotFoundException($"User with id {userId} not found");
            }

            return await dbContext.Devices.Include(d => d.User).Where(d => d.UserId == userId).ToListAsync();
        }

        public async Task<Device> AssignDeviceAsync(Guid deviceId, Guid userId)
        {
            var device = await GetByIdAsync(deviceId);

            var userExists = await dbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userExists)
            {
                throw new EntityNotFoundException($"User with id {userId} not found");
            }

            if (device.UserId != null)
            {
                throw new ResourceConflictException("Device is already assigned to a user");
            }

            device.UserId = userId;
            await dbContext.SaveChangesAsync();
            
            return device;
        }

        public async Task<Device> UnassignDeviceAsync(Guid deviceId, Guid userId)
        {
            var device = await GetByIdAsync(deviceId);

            var userExists = await dbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userExists)
            {
                throw new EntityNotFoundException($"User with id {userId} not found");
            }

            if (device.UserId != userId)
            {
                throw new ForbiddenAccessException("Cannot unassign a device you do not own");
            }

            device.UserId = null;
            await dbContext.SaveChangesAsync();
            
            return device;
        }
    }
}
