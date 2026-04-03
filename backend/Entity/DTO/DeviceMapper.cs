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
                UserId = device.UserId
            };
        }
    }
}
