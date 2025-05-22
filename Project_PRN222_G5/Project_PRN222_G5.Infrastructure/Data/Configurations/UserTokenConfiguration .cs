using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Infrastructure.Data.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RefreshToken)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ClientIp)
            .HasMaxLength(45)
            .IsRequired(false);

        builder.Property(x => x.ExpiredTime)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.UpdatedBy)
            .IsRequired(false);

        builder.HasOne(ut => ut.User)
            .WithMany(ut => ut.UserTokens)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}