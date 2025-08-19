using System;
using System.Collections.Generic;
using CentralizedApps.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Data;

public partial class CentralizedAppsDbContext : DbContext
{
    public CentralizedAppsDbContext()
    {
    }

    public CentralizedAppsDbContext(DbContextOptions<CentralizedAppsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Availibity> Availibities { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<ConfiguracionEmail> ConfiguracionEmails { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<MunicipalityProcedure> MunicipalityProcedures { get; set; }

    public virtual DbSet<MunicipalitySocialMedium> MunicipalitySocialMedia { get; set; }

    public virtual DbSet<NewsByMunicipality> NewsByMunicipalities { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<QueryField> QueryFields { get; set; }

    public virtual DbSet<ShieldMunicipality> ShieldMunicipalities { get; set; }

    public virtual DbSet<SocialMediaType> SocialMediaTypes { get; set; }

    public virtual DbSet<SportsFacility> SportsFacilities { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-CJE8DS1;Database=CentralizedApps;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Availibity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availibi__3214EC0769E16E7A");

            entity.ToTable("Availibity");

            entity.Property(e => e.TypeStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Banks__3214EC07FFE9D6AE");

            entity.Property(e => e.NameBank)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ConfiguracionEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Configur__3214EC075FE051B3");

            entity.HasIndex(e => e.Recurso, "IX_ConfiguracionEmails_Recurso");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Propiedad).HasMaxLength(100);
            entity.Property(e => e.Recurso).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC078E6DAF47");

            entity.Property(e => e.Get)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Post)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Municipality).WithMany(p => p.Courses)
                .HasForeignKey(d => d.MunicipalityId)
                .HasConstraintName("FK_CoursesToMunicipality");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC078392F4F6");

            entity.ToTable("Department");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07EE8D0B90");

            entity.ToTable("DocumentType");

            entity.Property(e => e.NameDocument)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Municipality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Municipa__3214EC07D1CB81CE");

            entity.ToTable("Municipality");

            entity.Property(e => e.Domain)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntityCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdBank).HasColumnName("Id_Bank");
            entity.Property(e => e.IdShield).HasColumnName("Id_Shield");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordFintech)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserFintech)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Municipality_ToDepartment");

            entity.HasOne(d => d.IdBankNavigation).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.IdBank)
                .HasConstraintName("FK_MunicipalityToBanks");

            entity.HasOne(d => d.IdShieldNavigation).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.IdShield)
                .HasConstraintName("FK_MunicipalityToShieldMunicipality");

            entity.HasOne(d => d.Theme).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK_Municipality_ToTheme");
        });

        modelBuilder.Entity<MunicipalityProcedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Municipa__3214EC07A2CEECA9");

            entity.ToTable("Municipality_Procedures");

            entity.Property(e => e.IntegrationType).IsUnicode(false);

            entity.HasOne(d => d.Municipality).WithMany(p => p.MunicipalityProcedures)
                .HasForeignKey(d => d.MunicipalityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MunicipalityProcedures_ToMunicipality");

            entity.HasOne(d => d.Procedures).WithMany(p => p.MunicipalityProcedures)
                .HasForeignKey(d => d.ProceduresId)
                .HasConstraintName("FK_MunicipalityProcedures_ToProcedures");
        });

        modelBuilder.Entity<MunicipalitySocialMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Municipa__3214EC0709387C37");

            entity.ToTable("Municipality_SocialMedia");

            entity.Property(e => e.Url)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("URL");

            entity.HasOne(d => d.Municipality).WithMany(p => p.MunicipalitySocialMedia)
                .HasForeignKey(d => d.MunicipalityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Municipality_SocialMedia_ToMunicipality");

            entity.HasOne(d => d.SocialMediaType).WithMany(p => p.MunicipalitySocialMedia)
                .HasForeignKey(d => d.SocialMediaTypeId)
                .HasConstraintName("FK_Municipality_SocialMedia_ToType");
        });

        modelBuilder.Entity<NewsByMunicipality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NewsByMu__3214EC0722834A37");

            entity.ToTable("NewsByMunicipality");

            entity.Property(e => e.GetUrlNew)
                .IsUnicode(false)
                .HasColumnName("Get_UrlNew");
            entity.Property(e => e.IdMunicipality).HasColumnName("Id_Municipality");

            entity.HasOne(d => d.IdMunicipalityNavigation).WithMany(p => p.NewsByMunicipalities)
                .HasForeignKey(d => d.IdMunicipality)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsByMunicipalityToMunicipality");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentH__3214EC07136307CD");

            entity.ToTable("PaymentHistory");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.Municipality).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.MunicipalityId)
                .HasConstraintName("FK_PaymentHistory_ToMunicipality");

            entity.HasOne(d => d.MunicipalityProcedures).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.MunicipalityProceduresId)
                .HasConstraintName("FK_PaymentHistory_ToMunicipalityProcedures");

            entity.HasOne(d => d.StatusTypeNavigation).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.StatusType)
                .HasConstraintName("FK_PaymentHistoryToAvailibity");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PaymentHistory_ToUser");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Procedur__3214EC0731F76909");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<QueryField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QueryFie__3214EC0784B181D2");

            entity.ToTable("QueryField");

            entity.Property(e => e.FieldName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Municipality).WithMany(p => p.QueryFields)
                .HasForeignKey(d => d.MunicipalityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_QueryField_ToMunicipality");
        });

        modelBuilder.Entity<ShieldMunicipality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shield_M__3214EC0771B5A892");

            entity.ToTable("Shield_Municipality");

            entity.Property(e => e.NameOfMunicipality)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Url).IsUnicode(false);
        });

        modelBuilder.Entity<SocialMediaType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SocialMe__3214EC07645744B3");

            entity.ToTable("SocialMediaType");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SportsFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SportsFa__3214EC0755B442C9");

            entity.Property(e => e.CalendaryPost)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Get)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReservationPost)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Municipality).WithMany(p => p.SportsFacilities)
                .HasForeignKey(d => d.MunicipalityId)
                .HasConstraintName("FK_SportsFacilityToMunicipality");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Theme__3214EC07D2DB093C");

            entity.ToTable("Theme");

            entity.Property(e => e.BackGroundColor)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NameTheme)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OnPrimaryColorDark)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.OnPrimaryColorLight)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryColor)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SecondaryColor)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SecondaryColorBlack)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0797A80A61");

            entity.Property(e => e.Address)
                .HasMaxLength(222)
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
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SecondLastName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Users)
                .HasForeignKey(d => d.DocumentTypeId)
                .HasConstraintName("FK_User_ToDocumentType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
