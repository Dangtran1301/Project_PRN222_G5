using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

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

        builder.Property(x => x.Roles)
            .HasMaxLength(10)
            .HasConversion(
                v => string.Join(",", v.Select(r => r.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Enum.Parse<Role>)
                    .ToList())
            .IsRequired();
    }
}