using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Users;
using System;

namespace Project_PRN222_G5.Infrastructure.Data;

public class TheDbContext : DbContext
{
    public TheDbContext(DbContextOptions<TheDbContext> options/*, IDatetimeService*/)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<UserResetPassword> UserResetPasswords { get; set; }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    //{
    //    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    //    {
    //        switch (entry.State)
    //        {
    //            case EntityState.Added:
    //                entry.Entity.CreatedAt = _dateTime.NowUtc;
    //                entry.Entity.CreatedBy = _authenticatedUser.UserId;
    //                break;
    //            case EntityState.Modified:
    //                entry.Entity.UpdatedAt = _dateTime.NowUtc;
    //                entry.Entity.UpdatedBy = _authenticatedUser.UserId;
    //                break;
    //        }
    //    }
    //}
}