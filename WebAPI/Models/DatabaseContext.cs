using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriesProductsMapping> CategoriesProductsMappings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<SystemLog> SystemLogs { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<Category>()
            .Property(e => e.CreatedAt)
            .HasColumnType("integer");
        modelBuilder.Entity<Category>()
            .Property(e => e.ModifiedAt)
            .HasColumnType("integer");
        modelBuilder.Entity<Category>()
            .Property(e => e.IsDeleted)
            .HasColumnType("integer");

        modelBuilder.Entity<Product>()
           .Property(e => e.CreatedAt)
           .HasColumnType("integer");
        modelBuilder.Entity<Product>()
            .Property(e => e.ModifiedAt)
            .HasColumnType("integer");
        modelBuilder.Entity<Product>()
            .Property(e => e.IsDeleted)
            .HasColumnType("integer");

        modelBuilder.Entity<SystemLog>()
          .Property(e => e.CreatedAt)
          .HasColumnType("integer");

        modelBuilder.Entity<User>()
          .Property(e => e.CreatedAt)
          .HasColumnType("integer");
        modelBuilder.Entity<User>()
            .Property(e => e.ModifiedAt)
            .HasColumnType("integer");
        modelBuilder.Entity<User>()
            .Property(e => e.IsDeleted)
            .HasColumnType("integer");

        modelBuilder.Entity<UserRole>()
          .Property(e => e.IsDeleted)
          .HasColumnType("integer");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
