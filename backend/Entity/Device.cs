namespace backend.Entity
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public DeviceType DeviceType { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string Processor {  get; set; }
        public double RamAmount { get; set; }
        public string Description { get; set; }
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
    }
}
