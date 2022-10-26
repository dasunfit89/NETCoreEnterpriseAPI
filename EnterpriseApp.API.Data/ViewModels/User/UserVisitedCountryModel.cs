using System;
namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserVisitedCountryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int SortOrder { get; set; }

        public string Nationality { get; set; }
    }
}
