using System;
namespace EnterpriseApp.API.Models.Entity
{
    public class ResListRestaurant :BaseEntity
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public int RestaurantId { get; set; }
         
        public virtual UserResList List { get; set; }

        public virtual Restaurant Restaurant { get; set; }

    }
}
