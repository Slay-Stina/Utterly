using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Utterly.Areas.Identity.Data;

namespace Utterly.Data;

public class UtterlyContext : IdentityDbContext<UtterlyUser>
{
    public UtterlyContext(DbContextOptions<UtterlyContext> options)
        : base(options)
    {
    }

    public DbSet<UtterlyPost> UtterlyPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UtterlyPost>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
