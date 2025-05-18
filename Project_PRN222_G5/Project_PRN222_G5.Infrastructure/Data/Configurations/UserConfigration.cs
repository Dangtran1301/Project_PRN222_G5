using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(100);

        builder.Property(x => x.Username)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Phone)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(x => x.DayOfBirth)
            .HasColumnType("date");

        builder.Property(x => x.UserStatus)
            .HasColumnType("tinyint");

        builder.Property(x => x.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}