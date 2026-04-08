using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entity
{
    public class Device
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }

        [Column("device_type")]
        public DeviceType? DeviceType { get; set; }
        public string? OS { get; set; }

        [Column("os_version")]
        public string? OSVersion { get; set; }
        public string? Processor { get; set; }

        [Column("ram_amount")]
        public decimal? RamAmount { get; set; }
        public string? Description { get; set; }

        [Column("user_id")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        public Device() { }

    }
}
