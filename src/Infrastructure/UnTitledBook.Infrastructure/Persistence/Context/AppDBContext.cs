using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UntitledBook.Domain.Entities;

namespace UnTitledBook.Infrastructure.Persistence.Context
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendship>()
           .HasOne(f => f.Friend)
           .WithMany()
           .HasForeignKey(f => f.FriendUserId)
           .OnDelete(DeleteBehavior.Restrict);

            /**/

            modelBuilder.Entity<Note>()
           .HasOne(n => n.User)
           .WithMany()
           .HasForeignKey(n => n.UserId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
           .HasOne(n => n.Friend)
           .WithMany()
           .HasForeignKey(n => n.FriendUserId)
           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
