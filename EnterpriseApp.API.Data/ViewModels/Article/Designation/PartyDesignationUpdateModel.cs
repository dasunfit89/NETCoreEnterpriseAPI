using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PartyDesignationUpdateModel : PartyDesignationInsertModel
    {
        [Required]
        public string Id { get; set; }
    }
}
