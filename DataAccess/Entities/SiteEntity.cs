using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class SiteEntity
    {
        public SiteEntity()
        {
            ReportingPeriodFacilityGasSupplyBreakDownEntities = new HashSet<ReportingPeriodFacilityGasSupplyBreakDownEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<ReportingPeriodFacilityGasSupplyBreakDownEntity> ReportingPeriodFacilityGasSupplyBreakDownEntities { get; set; }
    }
}
