using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Models.Entity
{
    public class ResSubcategoryEntity : BaseEntity
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public int SubcategoryId { get; set; }

        public virtual SubcategoryEntity Subcategory { get; set; }
    }
}
