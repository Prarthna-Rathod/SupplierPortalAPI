using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodFacilityElectricityGridMixEntity
    {
        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int ElectricityGridMixComponentId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int FercRegionId { get; set; }
        public decimal Content { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public virtual ElectricityGridMixComponentEntity ElectricityGridMixComponent { get; set; } = null!;
        public virtual FercRegionEntity FercRegion { get; set; } = null!;
        public virtual ReportingPeriodFacilityEntity ReportingPeriodFacility { get; set; } = null!;
        public virtual UnitOfMeasureEntity UnitOfMeasure { get; set; } = null!;
    }
}
