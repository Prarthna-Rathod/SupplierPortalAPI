using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodFacilityEntity
    {
        public ReportingPeriodFacilityEntity()
        {
            ReportingPeriodFacilityDocumentEntities = new HashSet<ReportingPeriodFacilityDocumentEntity>();
            ReportingPeriodFacilityElectricityGridMixEntities = new HashSet<ReportingPeriodFacilityElectricityGridMixEntity>();
        }

        public int Id { get; set; }
        public int FacilityId { get; set; }
        public int FacilityReportingPeriodDataStatusId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public int ReportingTypeId { get; set; }
        public string? GhgrpfacilityId { get; set; }
        public int SupplyChainStageId { get; set; }

        public int FercRegionId { get; set; }
        public bool IsActive { get; set; }
    

        public virtual FacilityEntity Facility { get; set; } = null!;
        public virtual FacilityReportingPeriodDataStatusEntity FacilityReportingPeriodDataStatus { get; set; } = null!;
        public virtual ReportingPeriodEntity ReportingPeriod { get; set; } = null!;
        public virtual ReportingPeriodSupplierEntity ReportingPeriodSupplier { get; set; } = null!;
        public virtual ReportingTypeEntity ReportingType { get; set; } = null!;
        public virtual SupplyChainStageEntity SupplyChainStage { get; set; } = null!;

        public virtual FercRegionEntity FercRegion { get; set; } = null!;
        public virtual ICollection<ReportingPeriodFacilityDocumentEntity> ReportingPeriodFacilityDocumentEntities { get; set; }
        public virtual ICollection<ReportingPeriodFacilityElectricityGridMixEntity> ReportingPeriodFacilityElectricityGridMixEntities { get; set; }
    }
}
