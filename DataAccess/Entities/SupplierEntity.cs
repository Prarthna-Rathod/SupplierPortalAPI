using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class SupplierEntity
    {
        public SupplierEntity()
        {
            ContactEntities = new HashSet<ContactEntity>();
            FacilityEntities = new HashSet<FacilityEntity>();
            ReportingPeriodSupplierEntities = new HashSet<ReportingPeriodSupplierEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ContactNo { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual ICollection<ContactEntity> ContactEntities { get; set; }
        public virtual ICollection<FacilityEntity> FacilityEntities { get; set; }
        public virtual ICollection<ReportingPeriodSupplierEntity> ReportingPeriodSupplierEntities { get; set; }
    }
}
