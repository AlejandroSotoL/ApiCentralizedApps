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

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<MunicipalityProcedure> MunicipalityProcedures { get; set; }

    public virtual DbSet<MunicipalitySocialMedium> MunicipalitySocialMedia { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<QueryField> QueryFields { get; set; }

    public virtual DbSet<SocialMediaType> SocialMediaTypes { get; set; }

    public virtual DbSet<SportsFacility> SportsFacilities { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Availibity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availibi__3214EC0737A07027");

            entity.ToTable("Availibity");

            entity.Property(e => e.TypeStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC0731B8B5A1");

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
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC073931A838");

            entity.ToTable("Department");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC078A980467");

            entity.ToTable("DocumentType");

            entity.Property(e => e.NameDocument)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Municipality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Municipa__3214EC076C4CC03D");

            entity.ToTable("Municipality");

            entity.Property(e => e.Domain)
                .HasMaxLength(100)
                .IsUnicode(false);
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

            entity.HasOne(d => d.Theme).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK_Municipality_ToTheme");
        });

        modelBuilder.Entity<MunicipalityProcedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Municipa__3214EC07FA7CB257");

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
            entity.HasKey(e => e.Id).HasName("PK__Municipa__3214EC0710E50210");

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

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentH__3214EC079CC035D2");

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
            entity.HasKey(e => e.Id).HasName("PK__Procedur__3214EC072406C282");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<QueryField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QueryFie__3214EC07960F56FE");

            entity.ToTable("QueryField");

            entity.Property(e => e.FieldName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Municipality).WithMany(p => p.QueryFields)
                .HasForeignKey(d => d.MunicipalityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_QueryField_ToMunicipality");
        });

        modelBuilder.Entity<SocialMediaType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SocialMe__3214EC07E0268BB1");

            entity.ToTable("SocialMediaType");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SportsFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SportsFa__3214EC07FEAC79FC");

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
            entity.HasKey(e => e.Id).HasName("PK__Theme__3214EC072F5BC665");

            entity.ToTable("Theme");

            entity.Property(e => e.BackGroundColor)
                .HasMaxLength(200)
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
            entity.Property(e => e.Shield).IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07D08C940C");

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
