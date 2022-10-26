using System;
namespace EnterpriseApp.API.Models.Entity
{
    public class UserReward : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int IconId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
