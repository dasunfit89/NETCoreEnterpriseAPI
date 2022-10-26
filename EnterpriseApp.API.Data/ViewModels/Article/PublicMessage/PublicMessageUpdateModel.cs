using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicMessageUpdateModel : PublicSentNewsInsertModel
    {
        [Required]
        public string Id { get; set; }
    }
}
