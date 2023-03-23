using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class ContactEntity
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual SupplierEntity Supplier { get; set; } = null!;
        public virtual UserEntity User { get; set; } = null!;
    }
}
