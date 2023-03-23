using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class DocumentRequiredStatusEntity
    {
        public DocumentRequiredStatusEntity()
        {
            FacilityRequiredDocumentTypeEntities = new HashSet<FacilityRequiredDocumentTypeEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<FacilityRequiredDocumentTypeEntity> FacilityRequiredDocumentTypeEntities { get; set; }
    }
}
