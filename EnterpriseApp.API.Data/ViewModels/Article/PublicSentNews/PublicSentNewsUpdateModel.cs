using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicSentNewsUpdateModel : PublicSentNewsInsertModel
    {
        [Required]
        public string Id { get; set; }
    }
}
