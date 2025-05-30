using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.DataAccess.Entities.Movies;

namespace Project_PRN222_G5.DataAccess.Data.Configurations;

public class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
{
    public void Configure(EntityTypeBuilder<Showtime> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.HasOne(x => x.Movie)
            .WithMany(x => x.Showtimes)
            .HasForeignKey(x => x.MovieId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Room)
            .WithMany(x => x.Showtimes)
            .HasForeignKey(x => x.RoomId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();
    }
}