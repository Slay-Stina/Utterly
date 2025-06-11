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
    public DbSet<Category> Categories { get; set; }
    public DbSet<UtterlyThread> Threads { get; set; }
    public DbSet<PersonalMessage> PersonalMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UtterlyPost>()
            .HasOne(p => p.Thread)
            .WithMany(t => t.Posts)
            .HasForeignKey(p => p.ThreadId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UtterlyPost>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UtterlyPost>()
            .HasOne<UtterlyPost>()
            .WithMany()
            .HasForeignKey(p => p.ParentPostId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PersonalMessage>()
            .HasOne(pm => pm.Sender)
            .WithMany()
            .HasForeignKey(pm => pm.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PersonalMessage>()
            .HasOne(pm => pm.Receiver)
            .WithMany()
            .HasForeignKey(pm => pm.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
