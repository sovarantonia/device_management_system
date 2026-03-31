using backend.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Repository
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.UserId)
                   .HasColumnName("user_id");

            builder.HasOne(d => d.User)
                   .WithMany(u => u.Devices)
                   .HasForeignKey(d => d.UserId);
        }
    }
}
