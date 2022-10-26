using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class UserType : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconId { get; set; }

        public virtual ICollection<UserUType> UserTypes { get; set; }
    }
}
