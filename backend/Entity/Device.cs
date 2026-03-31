using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entity
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        
        [Column("device_type")]
        public DeviceType DeviceType { get; set; }
        public string OS { get; set; }

        [Column("os_version")]
        public string OSVersion { get; set; }
        public string Processor {  get; set; }

        [Column("ram_amount")]
        public double RamAmount { get; set; }
        public string Description { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Device() { }

        public Device(Guid id, string name, string manufacturer, DeviceType deviceType, string oS, string oSVersion, string processor, double ramAmount, string description)
        {
            Id = id;
            Name = name;
            Manufacturer = manufacturer;
            DeviceType = deviceType;
            OS = oS;
            OSVersion = oSVersion;
            Processor = processor;
            RamAmount = ramAmount;
            Description = description;
        }

        public Device(Guid id, string name, string manufacturer, DeviceType deviceType, string oS, string oSVersion, string processor, double ramAmount, string description, User user) : this(id, name, manufacturer, deviceType, oS, oSVersion, processor, ramAmount, description)
        {
            User = user;
        }

        public override string ToString()
        {
            return $"Id: {Id}, name: {Name}, manufacturer: {Manufacturer}, device type: {DeviceType}, OS: {OS}, version: {OSVersion}" +
                $"processor: {Processor}, ram amount: {RamAmount}, description: {Description}, owner: {User?.ToString()}";
        }
    }
}
