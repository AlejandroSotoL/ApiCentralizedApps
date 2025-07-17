using System;
using System.Collections.Generic;
using CentralizedApps.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Infrastructure.Data;

public partial class CentralizedAppsDbContext : DbContext
{
    public CentralizedAppsDbContext()
    {
    }

    public CentralizedAppsDbContext(DbContextOptions<CentralizedAppsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC075E373C7B");

            entity.ToTable("DocumentType");

            entity.Property(e => e.DocumentName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079284E2F0");

            entity.HasIndex(e => e.NationalId, "UQ__Users__E9AA32FA1884C8B4").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NationalId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SecondLastName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Users)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_DocumentType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
