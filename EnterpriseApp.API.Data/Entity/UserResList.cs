using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class UserResList : BaseEntity
    {
        public int Id { get; set; }

        public string ListName { get; set; }

        public string IconId { get; set; }

        public string LColour { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<ResListRestaurant> ResListRestaurants { get; set; }

    }
}
