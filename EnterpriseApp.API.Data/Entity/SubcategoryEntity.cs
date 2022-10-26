using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class SubcategoryEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ResSubcategoryEntity> ResSubcategoryEntityList { get; set; }        
    }
}
