using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Entities.TypeConfiguration
{
    public class SurchageRateConfiguration : IEntityTypeConfiguration<SurchargeRate>
    {
        public void Configure(EntityTypeBuilder<SurchargeRate> builder)
        {
            builder
                .ToTable("SurchargeRate")
                .HasKey(k => k.Id);
            builder
                .Property(b => b.Id)
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            builder
                .Property(b => b.ProductTypeId)
                .HasColumnType("int")
                .IsRequired();
            builder
                .Property(b => b.Rate)
                .HasColumnType("decimal")
                .HasPrecision(18,6);
        }
    }
}
