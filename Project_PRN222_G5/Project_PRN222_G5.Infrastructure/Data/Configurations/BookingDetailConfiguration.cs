using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.DataAccess.Entities.Identities.Booking;

namespace Project_PRN222_G5.DataAccess.Data.Configurations;

public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
{
    public void Configure(EntityTypeBuilder<BookingDetail> builder)
    {
        builder.HasKey(x => new { x.BookingId, x.SeatId });

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.BookingDetails)
            .HasForeignKey(x => x.BookingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Seat)
            .WithMany(x => x.BookingDetails)
            .HasForeignKey(x => x.SeatId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}