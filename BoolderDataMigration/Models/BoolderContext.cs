using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BoolderDataMigration.Models;

public partial class BoolderContext : DbContext
{
    public BoolderContext()
    {
    }

    public BoolderContext(DbContextOptions<BoolderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Circuit> Circuits { get; set; }

    public virtual DbSet<Cluster> Clusters { get; set; }

    public virtual DbSet<Line> Lines { get; set; }

    public virtual DbSet<Poi> Pois { get; set; }

    public virtual DbSet<PoiRoute> PoiRoutes { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<Topo> Topos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=boolder.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("areas");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClusterId).HasColumnName("cluster_id");
            entity.Property(e => e.DescriptionEn).HasColumnName("description_en");
            entity.Property(e => e.DescriptionFr).HasColumnName("description_fr");
            entity.Property(e => e.DownloadSize).HasColumnName("download_size");
            entity.Property(e => e.Level1Count).HasColumnName("level1_count");
            entity.Property(e => e.Level2Count).HasColumnName("level2_count");
            entity.Property(e => e.Level3Count).HasColumnName("level3_count");
            entity.Property(e => e.Level4Count).HasColumnName("level4_count");
            entity.Property(e => e.Level5Count).HasColumnName("level5_count");
            entity.Property(e => e.Level6Count).HasColumnName("level6_count");
            entity.Property(e => e.Level7Count).HasColumnName("level7_count");
            entity.Property(e => e.Level8Count).HasColumnName("level8_count");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NameSearchable).HasColumnName("name_searchable");
            entity.Property(e => e.NorthEastLat).HasColumnName("north_east_lat");
            entity.Property(e => e.NorthEastLon).HasColumnName("north_east_lon");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.ProblemsCount).HasColumnName("problems_count");
            entity.Property(e => e.SouthWestLat).HasColumnName("south_west_lat");
            entity.Property(e => e.SouthWestLon).HasColumnName("south_west_lon");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.WarningEn).HasColumnName("warning_en");
            entity.Property(e => e.WarningFr).HasColumnName("warning_fr");
        });

        modelBuilder.Entity<Circuit>(entity =>
        {
            entity.ToTable("circuits");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AverageGrade).HasColumnName("average_grade");
            entity.Property(e => e.BeginnerFriendly).HasColumnName("beginner_friendly");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Dangerous).HasColumnName("dangerous");
            entity.Property(e => e.NorthEastLat).HasColumnName("north_east_lat");
            entity.Property(e => e.NorthEastLon).HasColumnName("north_east_lon");
            entity.Property(e => e.SouthWestLat).HasColumnName("south_west_lat");
            entity.Property(e => e.SouthWestLon).HasColumnName("south_west_lon");
        });

        modelBuilder.Entity<Cluster>(entity =>
        {
            entity.ToTable("clusters");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.MainAreaId).HasColumnName("main_area_id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Line>(entity =>
        {
            entity.ToTable("lines");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Coordinates).HasColumnName("coordinates");
            entity.Property(e => e.ProblemId).HasColumnName("problem_id");
            entity.Property(e => e.TopoId).HasColumnName("topo_id");
        });

        modelBuilder.Entity<Poi>(entity =>
        {
            entity.ToTable("pois");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GoogleUrl).HasColumnName("google_url");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PoiType).HasColumnName("poi_type");
            entity.Property(e => e.ShortName).HasColumnName("short_name");
        });

        modelBuilder.Entity<PoiRoute>(entity =>
        {
            entity.ToTable("poi_routes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.DistanceInMinutes).HasColumnName("distance_in_minutes");
            entity.Property(e => e.PoiId).HasColumnName("poi_id");
            entity.Property(e => e.Transport).HasColumnName("transport");
        });

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.ToTable("problems");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.BleauInfoId).HasColumnName("bleau_info_id");
            entity.Property(e => e.CircuitColor).HasColumnName("circuit_color");
            entity.Property(e => e.CircuitId).HasColumnName("circuit_id");
            entity.Property(e => e.CircuitNumber).HasColumnName("circuit_number");
            entity.Property(e => e.Featured).HasColumnName("featured");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NameEn).HasColumnName("name_en");
            entity.Property(e => e.NameSearchable).HasColumnName("name_searchable");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Popularity).HasColumnName("popularity");
            entity.Property(e => e.SitStart).HasColumnName("sit_start");
            entity.Property(e => e.Steepness).HasColumnName("steepness");
        });

        modelBuilder.Entity<Topo>(entity =>
        {
            entity.ToTable("topos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.BoulderId).HasColumnName("boulder_id");
            entity.Property(e => e.Position).HasColumnName("position");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
