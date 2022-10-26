using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class AddRestaurantCommentResponse
    {
        public bool IsSuccessful { get; set; }

        public ResCommentModel Comment { get; set; }
    }
}