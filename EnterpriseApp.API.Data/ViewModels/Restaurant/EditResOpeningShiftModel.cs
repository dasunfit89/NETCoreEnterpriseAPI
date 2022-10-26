using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class EditResOpeningShiftModel
    {
        public int Id { get; set; }

        public string Session { get; set; }

        public string Day { get; set; }

        public string Opens { get; set; }

        public string Closes { get; set; }

        public int RestaurantId { get; set; }
    }
}
