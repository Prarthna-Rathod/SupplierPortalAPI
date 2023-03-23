using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodEntity
    {
        public ReportingPeriodEntity()
        {
            ReportingPeriodSupplierEntities = new HashSet<ReportingPeriodSupplierEntity>();
        }

        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public int ReportingPeriodTypeId { get; set; }
        public string CollectionTimePeriod { get; set; } = null!;
        public int ReportingPeriodStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual ReportingPeriodStatusEntity ReportingPeriodStatus { get; set; } = null!;
        public virtual ReportingPeriodTypeEntity ReportingPeriodType { get; set; } = null!;
        public virtual ICollection<ReportingPeriodSupplierEntity> ReportingPeriodSupplierEntities { get; set; }
    }
}
