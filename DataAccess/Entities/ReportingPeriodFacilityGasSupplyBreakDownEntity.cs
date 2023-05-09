using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodFacilityGasSupplyBreakDownEntity
    {
        public int Id { get; set; }
        public int PeriodFacilityId { get; set; }
        public int SiteId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public decimal Content { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;

        public virtual ReportingPeriodFacilityEntity PeriodFacility { get; set; } = null!;
        public virtual SiteEntity Site { get; set; } = null!;
        public virtual UnitOfMeasureEntity UnitOfMeasure { get; set; } = null!;
    }
}
