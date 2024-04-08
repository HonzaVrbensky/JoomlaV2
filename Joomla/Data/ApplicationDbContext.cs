using Joomla.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Joomla.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<JUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<JUser> JUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<JUser>().ToTable("Users");
        builder.Entity<JUser>().Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Entity<Article>().Property(a => a.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        PasswordHasher<JUser> hasher = new PasswordHasher<JUser>();

        Guid adminRoleId = Guid.NewGuid();
        Guid adminId = Guid.NewGuid();
        builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>
        {
            Id = adminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        });

        builder.Entity<JUser>().HasData(new JUser
        {
            Id = adminId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@localhost",
            NormalizedEmail = "ADMIN@LOCALHOST",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(new JUser(), "admin"),
            SecurityStamp = string.Empty,
            FullName = "Administrator",
            BirthDate = DateTime.Parse("2000-01-01")
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = adminRoleId,
            UserId = adminId
        });
    }
}
