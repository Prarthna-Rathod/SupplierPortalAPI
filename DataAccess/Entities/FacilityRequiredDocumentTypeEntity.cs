using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class FacilityRequiredDocumentTypeEntity
    {
        public int Id { get; set; }
        public int ReportingTypeId { get; set; }
        public int SupplyChainStageId { get; set; }
        public int DocumentTypeId { get; set; }
        public int DocumentRequiredStatusId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;

        public virtual DocumentRequiredStatusEntity DocumentRequiredStatus { get; set; } = null!;
        public virtual DocumentTypeEntity DocumentType { get; set; } = null!;
        public virtual ReportingTypeEntity ReportingType { get; set; } = null!;
        public virtual SupplyChainStageEntity SupplyChainStage { get; set; } = null!;
    }
}
