using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class ResCommentRequest
    {
        [Required]
        public List<string> RProblems { get; set; }

        public string ProblemTxt { get; set; }
        
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }
    }
}
