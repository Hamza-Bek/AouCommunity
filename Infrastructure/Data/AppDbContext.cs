using Domain.Models;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thread = Domain.Models.ChatModels.Thread;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<AvailableUser> AvailableUsers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ThreadRequest> ThreadRequests { get; set; }
    public DbSet<Thread> Threads { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(p => p.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);



    }
}