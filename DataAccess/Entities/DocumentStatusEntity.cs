using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class DocumentStatusEntity
    {
        public DocumentStatusEntity()
        {
            ReportingPeriodFacilityDocumentEntities = new HashSet<ReportingPeriodFacilityDocumentEntity>();
            ReportingPeriodSupplierDocumentEntities = new HashSet<ReportingPeriodSupplierDocumentEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<ReportingPeriodFacilityDocumentEntity> ReportingPeriodFacilityDocumentEntities { get; set; }
        public virtual ICollection<ReportingPeriodSupplierDocumentEntity> ReportingPeriodSupplierDocumentEntities { get; set; }
    }
}
