using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class SupplierReportingPeriodStatusEntity
    {
        public SupplierReportingPeriodStatusEntity()
        {
            ReportingPeriodSupplierEntities = new HashSet<ReportingPeriodSupplierEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<ReportingPeriodSupplierEntity> ReportingPeriodSupplierEntities { get; set; }
    }
}
