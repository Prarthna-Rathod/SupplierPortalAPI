using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ReportingTypeEntity
    {
        public ReportingTypeEntity()
        {
            FacilityEntities = new HashSet<FacilityEntity>();
            FacilityRequiredDocumentTypeEntities = new HashSet<FacilityRequiredDocumentTypeEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<FacilityEntity> FacilityEntities { get; set; }
        public virtual ICollection<FacilityRequiredDocumentTypeEntity> FacilityRequiredDocumentTypeEntities { get; set; }
    }
}
