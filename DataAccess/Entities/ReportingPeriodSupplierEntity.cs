using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodSupplierEntity
    {
        public ReportingPeriodSupplierEntity()
        {
            ReportingPeriodFacilityEntities = new HashSet<ReportingPeriodFacilityEntity>();
            ReportingPeriodSupplierDocumentEntities = new HashSet<ReportingPeriodSupplierDocumentEntity>();
        }

        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierReportingPeriodStatusId { get; set; }
        public DateTime? InitialDataRequestDate { get; set; }
        public DateTime? ResendDataRequestDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ReportingPeriodEntity ReportingPeriod { get; set; } = null!;
        public virtual SupplierEntity Supplier { get; set; } = null!;
        public virtual SupplierReportingPeriodStatusEntity SupplierReportingPeriodStatus { get; set; } = null!;
        public virtual ICollection<ReportingPeriodFacilityEntity> ReportingPeriodFacilityEntities { get; set; }
        public virtual ICollection<ReportingPeriodSupplierDocumentEntity> ReportingPeriodSupplierDocumentEntities { get; set; }
    }
}
