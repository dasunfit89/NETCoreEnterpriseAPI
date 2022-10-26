using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class EditResOpeningDayModel
    {
        public string Weekday { get; set; }

        public IEnumerable<EditResOpeningShiftModel> Shifts { get; set; }
    }
}
