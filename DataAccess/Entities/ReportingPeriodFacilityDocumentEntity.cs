using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodFacilityDocumentEntity
    {
        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public string Version { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string StoredName { get; set; } = null!;
        public string Path { get; set; } = null!;
        public int DocumentStatusId { get; set; }
        public int DocumentTypeId { get; set; }
        public string ValidationError { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual DocumentStatusEntity DocumentStatus { get; set; } = null!;
        public virtual DocumentTypeEntity DocumentType { get; set; } = null!;
        public virtual ReportingPeriodFacilityEntity ReportingPeriodFacility { get; set; } = null!;
    }
}
