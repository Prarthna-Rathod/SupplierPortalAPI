using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.DataActionContext;

public partial class SupplierPortalDBContext : DbContext
{
    public SupplierPortalDBContext()
    {
    }

    public SupplierPortalDBContext(DbContextOptions<SupplierPortalDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssociatePipelineEntity> AssociatePipelineEntities { get; set; } = null!;
    public virtual DbSet<ContactEntity> ContactEntities { get; set; } = null!;
    public virtual DbSet<DocumentRequiredStatusEntity> DocumentRequiredStatusEntities { get; set; } = null!;
    public virtual DbSet<DocumentStatusEntity> DocumentStatusEntities { get; set; } = null!;
    public virtual DbSet<DocumentTypeEntity> DocumentTypeEntities { get; set; } = null!;
    public virtual DbSet<ElectricityGridMixComponentEntity> ElectricityGridMixComponentEntities { get; set; } = null!;
    public virtual DbSet<FacilityEntity> FacilityEntities { get; set; } = null!;
    public virtual DbSet<FacilityReportingPeriodDataStatusEntity> FacilityReportingPeriodDataStatusEntities { get; set; } = null!;
    public virtual DbSet<FacilityRequiredDocumentTypeEntity> FacilityRequiredDocumentTypeEntities { get; set; } = null!;
    public virtual DbSet<FercRegionEntity> FercRegionEntities { get; set; } = null!;
    public virtual DbSet<Log> Logs { get; set; } = null!;
    public virtual DbSet<ReportingPeriodEntity> ReportingPeriodEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodFacilityDocumentEntity> ReportingPeriodFacilityDocumentEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodFacilityElectricityGridMixEntity> ReportingPeriodFacilityElectricityGridMixEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodFacilityEntity> ReportingPeriodFacilityEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodFacilityGasSupplyBreakDownEntity> ReportingPeriodFacilityGasSupplyBreakDownEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodStatusEntity> ReportingPeriodStatusEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodSupplierDocumentEntity> ReportingPeriodSupplierDocumentEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodSupplierEntity> ReportingPeriodSupplierEntities { get; set; } = null!;
    public virtual DbSet<ReportingPeriodTypeEntity> ReportingPeriodTypeEntities { get; set; } = null!;
    public virtual DbSet<ReportingTypeEntity> ReportingTypeEntities { get; set; } = null!;
    public virtual DbSet<RoleEntity> RoleEntities { get; set; } = null!;
    public virtual DbSet<SiteEntity> SiteEntities { get; set; } = null!;
    public virtual DbSet<SupplierEntity> SupplierEntities { get; set; } = null!;
    public virtual DbSet<SupplierReportingPeriodStatusEntity> SupplierReportingPeriodStatusEntities { get; set; } = null!;
    public virtual DbSet<SupplyChainStageEntity> SupplyChainStageEntities { get; set; } = null!;
    public virtual DbSet<UnitOfMeasureEntity> UnitOfMeasureEntities { get; set; } = null!;
    public virtual DbSet<UserEntity> UserEntities { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("name=SupplierConnnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssociatePipelineEntity>(entity =>
        {
            entity.ToTable("AssociatePipelineEntity");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContactEntity>(entity =>
        {
            entity.ToTable("ContactEntity");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.ContactEntities)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contact_Supplier");

            entity.HasOne(d => d.User)
                .WithMany(p => p.ContactEntities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contact_User");
        });

        modelBuilder.Entity<DocumentRequiredStatusEntity>(entity =>
        {
            entity.ToTable("DocumentRequiredStatusEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DocumentStatusEntity>(entity =>
        {
            entity.ToTable("DocumentStatusEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DocumentTypeEntity>(entity =>
        {
            entity.ToTable("DocumentTypeEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ElectricityGridMixComponentEntity>(entity =>
        {
            entity.ToTable("ElectricityGridMixComponentEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<FacilityEntity>(entity =>
        {
            entity.ToTable("FacilityEntity");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.GhgrpfacilityId)
                .HasMaxLength(100)
                .HasColumnName("GHGRPFacilityId");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.AssociatePipeline)
                .WithMany(p => p.FacilityEntities)
                .HasForeignKey(d => d.AssociatePipelineId)
                .HasConstraintName("FK_Facility_AssociatePipeline");

            entity.HasOne(d => d.ReportingType)
                .WithMany(p => p.FacilityEntities)
                .HasForeignKey(d => d.ReportingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Facility_ReportingType");

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.FacilityEntities)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityEntity_Supplier");

            entity.HasOne(d => d.SupplyChainStage)
                .WithMany(p => p.FacilityEntities)
                .HasForeignKey(d => d.SupplyChainStageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Facility_SupplyChainStage");
        });

        modelBuilder.Entity<FacilityReportingPeriodDataStatusEntity>(entity =>
        {
            entity.ToTable("FacilityReportingPeriodDataStatusEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<FacilityRequiredDocumentTypeEntity>(entity =>
        {
            entity.ToTable("FacilityRequiredDocumentTypeEntity");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.DocumentRequiredStatus)
                .WithMany(p => p.FacilityRequiredDocumentTypeEntities)
                .HasForeignKey(d => d.DocumentRequiredStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityRequiredDocumentType_DocumentRequiredStatus");

            entity.HasOne(d => d.DocumentType)
                .WithMany(p => p.FacilityRequiredDocumentTypeEntities)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityRequiredDocumentType_DocumentType");

            entity.HasOne(d => d.ReportingType)
                .WithMany(p => p.FacilityRequiredDocumentTypeEntities)
                .HasForeignKey(d => d.ReportingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityRequiredDocumentType_ReportingType");

            entity.HasOne(d => d.SupplyChainStage)
                .WithMany(p => p.FacilityRequiredDocumentTypeEntities)
                .HasForeignKey(d => d.SupplyChainStageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityRequiredDocumentType_SupplyChainStage");
        });

        modelBuilder.Entity<FercRegionEntity>(entity =>
        {
            entity.ToTable("FercRegionEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<ReportingPeriodEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodEntity");

            entity.Property(e => e.CollectionTimePeriod).HasMaxLength(100);

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.EndDate).HasColumnType("datetime");

            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.ReportingPeriodStatus)
                .WithMany(p => p.ReportingPeriodEntities)
                .HasForeignKey(d => d.ReportingPeriodStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriod_ReportingPeriodStatus");

            entity.HasOne(d => d.ReportingPeriodType)
                .WithMany(p => p.ReportingPeriodEntities)
                .HasForeignKey(d => d.ReportingPeriodTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriod_ReportingPeriodType");
        });

        modelBuilder.Entity<ReportingPeriodFacilityDocumentEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodFacilityDocumentEntity");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            //entity.Property(e => e.Version).HasMaxLength(50);

            entity.HasOne(d => d.DocumentStatus)
                .WithMany(p => p.ReportingPeriodFacilityDocumentEntities)
                .HasForeignKey(d => d.DocumentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityDocument_DocumentStatus");

            entity.HasOne(d => d.DocumentType)
                .WithMany(p => p.ReportingPeriodFacilityDocumentEntities)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityDocument_DocumentType");

            entity.HasOne(d => d.ReportingPeriodFacility)
                .WithMany(p => p.ReportingPeriodFacilityDocumentEntities)
                .HasForeignKey(d => d.ReportingPeriodFacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityDocument_ReportingPeriodFacility");
        });

        modelBuilder.Entity<ReportingPeriodFacilityElectricityGridMixEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodFacilityElectricityGridMixEntity");

            entity.Property(e => e.Content).HasColumnType("decimal(30, 20)");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.ElectricityGridMixComponent)
                .WithMany(p => p.ReportingPeriodFacilityElectricityGridMixEntities)
                .HasForeignKey(d => d.ElectricityGridMixComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityElectricityGridMixEntity_ElectricityGridMixComponentEntity");

            entity.HasOne(d => d.ReportingPeriodFacility)
                .WithMany(p => p.ReportingPeriodFacilityElectricityGridMixEntities)
                .HasForeignKey(d => d.ReportingPeriodFacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityElectricityGridMixEntity_ReportingPeriodFacilityEntity");

            entity.HasOne(d => d.UnitOfMeasure)
                .WithMany(p => p.ReportingPeriodFacilityElectricityGridMixEntities)
                .HasForeignKey(d => d.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityElectricityGridMixEntity_UnitOfMeasureEntity");
        });

        modelBuilder.Entity<ReportingPeriodFacilityEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodFacilityEntity");

            entity.Property(e => e.GhgrpfacilityId)
                .HasMaxLength(50)
                .HasColumnName("GHGRPFacilityId");

            entity.HasOne(d => d.Facility)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacility_Facility");

            entity.HasOne(d => d.FacilityReportingPeriodDataStatus)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.FacilityReportingPeriodDataStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacility_FacilityReportingPeriodDataStatus");

            entity.HasOne(d => d.FercRegion)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.FercRegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityEntity_FercRegion");

            entity.HasOne(d => d.ReportingPeriod)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.ReportingPeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacility_ReportingPeriod");

            entity.HasOne(d => d.ReportingPeriodSupplier)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.ReportingPeriodSupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacility_ReportingPeriodSupplier");

            entity.HasOne(d => d.ReportingType)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.ReportingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityEntity_ReportingType");

            entity.HasOne(d => d.SupplyChainStage)
                .WithMany(p => p.ReportingPeriodFacilityEntities)
                .HasForeignKey(d => d.SupplyChainStageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityEntity_SupplyChainStage");
        });

        modelBuilder.Entity<ReportingPeriodFacilityGasSupplyBreakDownEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodFacilityGasSupplyBreakDownEntity");

            entity.Property(e => e.Content).HasColumnType("decimal(30, 20)");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PeriodFacility)
                .WithMany(p => p.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                .HasForeignKey(d => d.PeriodFacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityGasSupplyBreakDownEntity_ReportingPeriodFacilityEntity");

            entity.HasOne(d => d.Site)
                .WithMany(p => p.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityGasSupplyBreakDownEntity_SiteEntity");

            entity.HasOne(d => d.UnitOfMeasure)
                .WithMany(p => p.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                .HasForeignKey(d => d.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodFacilityGasSupplyBreakDownEntity_UnitOfMeasureEntity");
        });

        modelBuilder.Entity<ReportingPeriodStatusEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodStatusEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ReportingPeriodSupplierDocumentEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodSupplierDocumentEntity");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.Property(e => e.Version).HasMaxLength(50);

            entity.HasOne(d => d.DocumentStatus)
                .WithMany(p => p.ReportingPeriodSupplierDocumentEntities)
                .HasForeignKey(d => d.DocumentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodSupplierDocument_DocumentStatus");

            entity.HasOne(d => d.DocumentType)
                .WithMany(p => p.ReportingPeriodSupplierDocumentEntities)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodSupplierDocument_DocumentType");

            entity.HasOne(d => d.ReportingPeriodSupplier)
                .WithMany(p => p.ReportingPeriodSupplierDocumentEntities)
                .HasForeignKey(d => d.ReportingPeriodSupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodSupplierDocument_ReportingPeriodSupplier");
        });

        modelBuilder.Entity<ReportingPeriodSupplierEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodSupplierEntity");

            entity.Property(e => e.InitialDataRequestDate).HasColumnType("datetime");

            entity.Property(e => e.ResendDataRequestDate).HasColumnType("datetime");

            entity.HasOne(d => d.ReportingPeriod)
                .WithMany(p => p.ReportingPeriodSupplierEntities)
                .HasForeignKey(d => d.ReportingPeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodSupplier_ReportingPeriod");

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.ReportingPeriodSupplierEntities)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodSupplier_Supplier");

            entity.HasOne(d => d.SupplierReportingPeriodStatus)
                .WithMany(p => p.ReportingPeriodSupplierEntities)
                .HasForeignKey(d => d.SupplierReportingPeriodStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportingPeriodSupplier_SupplierReportingPeriodStatus");
        });

        modelBuilder.Entity<ReportingPeriodTypeEntity>(entity =>
        {
            entity.ToTable("ReportingPeriodTypeEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ReportingTypeEntity>(entity =>
        {
            entity.ToTable("ReportingTypeEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.ToTable("RoleEntity");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SiteEntity>(entity =>
        {
            entity.ToTable("SiteEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.ToTable("SupplierEntity");

            entity.Property(e => e.Alias).HasMaxLength(100);

            entity.Property(e => e.ContactNo).HasMaxLength(50);

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Email).HasMaxLength(100);

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<SupplierReportingPeriodStatusEntity>(entity =>
        {
            entity.ToTable("SupplierReportingPeriodStatusEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SupplyChainStageEntity>(entity =>
        {
            entity.ToTable("SupplyChainStageEntity");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<UnitOfMeasureEntity>(entity =>
        {
            entity.ToTable("UnitOfMeasureEntity");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("UserEntity");

            entity.Property(e => e.ContactNo).HasMaxLength(50);

            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Email).HasMaxLength(100);

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserEntities)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


