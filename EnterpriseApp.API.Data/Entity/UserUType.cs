namespace EnterpriseApp.API.Models.Entity
{
    public class UserUType : BaseEntity
    {
        public int UserId { get; set; }

        public int UTypeId { get; set; }

        public virtual User User { get; set; }

        public virtual UserType UType { get; set; }
          
        public UserUType()
        {
        }
    }
}
