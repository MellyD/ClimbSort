using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace FontRecommender.Data
{
    public class FontRecommendationDBContext: DbContext
    {
        public FontRecommendationDBContext(DbContextOptions<FontRecommendationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topography>(entity =>
            {
                entity
                .HasMany(s => s.Coordinates)
                .WithOne(c => c.Topography)
                .OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("Topography").HasIndex(s => s.ModifiedDate).IsDescending(true);
            });
            modelBuilder.Entity<GradingSystem>().ToTable("GradingSystem");
            modelBuilder.Entity<WallType>().ToTable("WallType");
            modelBuilder.Entity<Crag>(entity =>
            {
                entity
                .HasMany(s => s.Coordinates)
                .WithOne(c => c.Crag)
                .OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("Crag").HasIndex(s => s.ModifiedDate).IsDescending(true);
            });
            modelBuilder.Entity<Coordinates>().ToTable("Coordinates");
            modelBuilder.Entity<Grade>(entity =>
            {
                entity
                .HasOne(b => b.GradingSystem)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("Grade").HasIndex(s => s.ScaleOrder).IsDescending(false);

            });
            modelBuilder.Entity<Climb>(entity =>
            {
                entity.ToTable("Climb").HasIndex(s => s.ModifiedDate).IsDescending(true);

                entity
                .HasMany(s => s.Coordinates)
                .WithOne(c => c.Climb)
                .OnDelete(DeleteBehavior.NoAction);

                entity
                .HasOne(s => s.Grade)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); 

                entity
                .HasOne(s => s.Topography)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientCascade);

                entity
                .HasOne(s => s.WallType)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

                entity
                .HasOne(s => s.Crag)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            });
        }
    }
}
