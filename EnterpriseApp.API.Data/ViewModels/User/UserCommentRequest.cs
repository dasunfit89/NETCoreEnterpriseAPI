using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UserCommentRequest
    {
        [Required]
        public List<string> Problems { get; set; }

        public string ProblemTxt { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ArticleId { get; set; }
        public object Email { get; set; }
    }
}
