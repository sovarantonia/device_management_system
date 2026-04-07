namespace backend.Entity.DTO
{
    public static class DeviceMapper
    {
        public static DeviceResponse ToDTO(Device device)
        {
            return new DeviceResponse
            {
                Id = device.Id,
                Name = device.Name,
                Manufacturer = device.Manufacturer,
                DeviceType = device.DeviceType.ToString(),
                OS = device.OS,
                OSVersion = device.OSVersion,
                Processor = device.Processor,
                RamAmount = device.RamAmount,
                Description = device.Description,
                User = device.User == null ? null : new UserResponse
                {
                    Id = device.User.Id,
                    Name = device.User.Name,
                    Email = device.User.Email,
                    Location = device.User.Location,
                }
            };
        }
    }
}
