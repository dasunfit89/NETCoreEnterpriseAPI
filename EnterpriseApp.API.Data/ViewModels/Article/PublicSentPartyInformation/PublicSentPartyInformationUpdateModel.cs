using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicSentPartyInformationUpdateModel : PublicSentPartyInformationInsertModel
    {
        [Required]
        public string Id { get; set; }
    }
}
