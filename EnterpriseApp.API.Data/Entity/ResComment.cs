using System;
namespace EnterpriseApp.API.Models.Entity
{
    public class ResComment : BaseEntity
    {
        public int Id { get; set; }
         
        public string RComment { get; set; }

        public DateTimeOffset RCDate { get; set; }

        public int RestaurantId { get; set; }

        public int UserId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual User User { get; set; }
    }
}
