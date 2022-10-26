using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class ResBadgeEntity : BaseEntity
    {
        public int Id { get; set; }

        public int BadgeId { get; set; }

        public virtual BadgeEntity Badge { get; set; }
        
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
