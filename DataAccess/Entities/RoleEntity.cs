using System;
using System.Collections.Generic;

namespace DataAccess.Entities
{
    public partial class RoleEntity
    {
        public RoleEntity()
        {
            UserEntities = new HashSet<UserEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<UserEntity> UserEntities { get; set; }
    }
}
