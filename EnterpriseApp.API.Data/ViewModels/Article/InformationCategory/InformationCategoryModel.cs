using System;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class InformationCategoryModel : BaseEntityModel
    {
        public string NameEn { get; set; }

        public string NameSi { get; set; }

        public int Order { get; set; }
    }
}
