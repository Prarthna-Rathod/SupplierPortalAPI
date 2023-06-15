using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingPeriodSupplierDocumentEntity
    {
        public int Id { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public int Version { get; set; }
        public string DisplayName { get; set; } = null!;
        public string StoredName { get; set; } = null!;
        public string? Path { get; set; }
        public int DocumentStatusId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? ValidationError { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual DocumentStatusEntity DocumentStatus { get; set; } = null!;
        public virtual DocumentTypeEntity DocumentType { get; set; } = null!;
        public virtual ReportingPeriodSupplierEntity ReportingPeriodSupplier { get; set; } = null!;
    }
}
