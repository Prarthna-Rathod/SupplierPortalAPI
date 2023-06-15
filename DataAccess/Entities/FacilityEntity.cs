using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class FacilityEntity
    {
        public FacilityEntity()
        {
            ReportingPeriodFacilityEntities = new HashSet<ReportingPeriodFacilityEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public string? GhgrpfacilityId { get; set; }
        public int SupplierId { get; set; }
        public int? AssociatePipelineId { get; set; }
        public int ReportingTypeId { get; set; }
        public int SupplyChainStageId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual AssociatePipelineEntity? AssociatePipeline { get; set; }
        public virtual ReportingTypeEntity ReportingType { get; set; } = null!;
        public virtual SupplierEntity Supplier { get; set; } = null!;
        public virtual SupplyChainStageEntity SupplyChainStage { get; set; } = null!;

        public virtual ICollection<ReportingPeriodFacilityEntity> ReportingPeriodFacilityEntities { get; set; }
    }
}
