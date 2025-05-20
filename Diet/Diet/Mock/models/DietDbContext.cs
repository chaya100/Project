using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mock.models;

public partial class DietDbContext : DbContext
{
    public DietDbContext()
    {
    }

    public DietDbContext(DbContextOptions<DietDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FoodItem> FoodItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=CHAYA-LURIA;database=dietDB;trusted_connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("foodItem");

            entity.Property(e => e.Category).HasMaxLength(255);
            entity.Property(e => e.Food).HasMaxLength(255);
            entity.Property(e => e.Measure).HasMaxLength(255);
            entity.Property(e => e.SatFat).HasColumnName("Sat#Fat");
            entity.Property(e => e.SourceFile)
                .HasMaxLength(255)
                .HasColumnName("Source_File");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
