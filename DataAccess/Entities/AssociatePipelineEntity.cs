using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class AssociatePipelineEntity
    {
        public AssociatePipelineEntity()
        {
            FacilityEntities = new HashSet<FacilityEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual ICollection<FacilityEntity> FacilityEntities { get; set; }
    }
}
