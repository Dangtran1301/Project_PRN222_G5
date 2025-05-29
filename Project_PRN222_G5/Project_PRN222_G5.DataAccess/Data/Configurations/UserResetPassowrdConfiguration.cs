using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_PRN222_G5.DataAccess.Entities.Users;

namespace Project_PRN222_G5.DataAccess.Data.Configurations
{
    internal class UserResetPasswordConfiguration : IEntityTypeConfiguration<UserResetPassword>
    {
        public void Configure(EntityTypeBuilder<UserResetPassword> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserResetPasswords)
                .HasForeignKey(x => x.UserId);
        }
    }
}