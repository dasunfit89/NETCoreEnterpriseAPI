using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class ResOpeningDayModel
    { 
        public string Weekday { get; set; }

        public IEnumerable<ResOpeningShiftModel> Shifts { get; set; }
    }
}
