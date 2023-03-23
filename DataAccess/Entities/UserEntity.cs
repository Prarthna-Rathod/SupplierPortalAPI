using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class UserEntity
    {
        public UserEntity()
        {
            ContactEntities = new HashSet<ContactEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ContactNo { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public virtual RoleEntity Role { get; set; } = null!;
        public virtual ICollection<ContactEntity> ContactEntities { get; set; }
    }
}
