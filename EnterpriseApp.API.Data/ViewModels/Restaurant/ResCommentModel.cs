using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class ResCommentModel
    {
        public int Id { get; set; }

        public string UEmail { get; set; }

        public string RComment { get; set; }

        public DateTimeOffset RCDate { get; set; }

        public int RestaurantId { get; set; }

        public int UserId { get; set; }

        public string RestaurantName { get; set; }

        public string UFirstName { get; set; }

        public string ULastName { get; set; }
    }
}
