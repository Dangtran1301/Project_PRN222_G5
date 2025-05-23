using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.Domain.Entities.Cinema;

namespace Project_PRN222_G5.Infrastructure.Data.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SeatNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.HasOne(x => x.Room)
            .WithMany(x => x.Seats)
            .HasForeignKey(x => x.RoomId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();
    }
}