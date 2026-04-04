namespace backend.Entity.DTO
{
    public class DeviceResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public string? DeviceType { get; set; }
        public string? OS { get; set; }
        public string? OSVersion { get; set; }
        public string? Processor { get; set; }
        public decimal? RamAmount { get; set; }
        public string? Description { get; set; }
        public UserSummary? User { get; set; }
    }
}
