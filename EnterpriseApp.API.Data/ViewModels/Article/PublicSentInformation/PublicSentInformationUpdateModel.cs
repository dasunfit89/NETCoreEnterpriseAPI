using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicSentInformationUpdateModel : PublicSentInformationInsertModel
    {
        [Required]
        public string Id { get; set; }
    }
}
