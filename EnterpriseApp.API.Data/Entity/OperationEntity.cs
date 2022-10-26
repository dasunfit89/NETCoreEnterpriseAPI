using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class BadgeEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconId { get; set; }
        
        public virtual ICollection<ResBadgeEntity> ResBadgeEntity { get; set; }
    }
}
