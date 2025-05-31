using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Utterly.Areas.Identity.Data;

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
