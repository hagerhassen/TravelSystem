using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.DataAccessLayer.Database
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TripPost> TripPosts {get;set;}
        public DbSet<LikedPostTable> LikedPosts { get; set; }
        public DbSet<DislikedPostTable> dislikedPosts { get; set; }
        public DbSet<SavedPostTable> SavedPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TripPost>(trip =>
            {
                trip.Property(p => p.Accepted).HasDefaultValue(false);
            });

            builder.Entity<LikedPostTable>().
                HasKey(e => new { e.postID, e.userID });

            builder.Entity<LikedPostTable>()
                .HasOne(e => e.user)
                .WithMany(e => e.LikedPosts)
                .HasForeignKey(e => e.userID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LikedPostTable>()
                .HasOne(e => e.post)
                .WithMany(e => e.Likedby)
                .HasForeignKey(e => e.postID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DislikedPostTable>().
                HasKey(e => new { e.postID, e.userID });

            builder.Entity<DislikedPostTable>()
                .HasOne(e => e.user)
                .WithMany(e => e.DislikedPosts)
                .HasForeignKey(e => e.userID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DislikedPostTable>()
                .HasOne(e => e.post)
                .WithMany(e => e.Dislikedby)
                .HasForeignKey(e => e.postID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SavedPostTable>().
                HasKey(e => new { e.postID, e.userID });

            builder.Entity<SavedPostTable>()
                .HasOne(e => e.user)
                .WithMany(e => e.SavedPosts)
                .HasForeignKey(e => e.userID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SavedPostTable>()
                .HasOne(e => e.post)
                .WithMany(e => e.Savedby)
                .HasForeignKey(e => e.postID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<ApplicationUser>().HasMany(user => user.Posted)
                .WithOne(trip => trip.Owner)
                .HasForeignKey(trip => trip.OwnerID)
                .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
