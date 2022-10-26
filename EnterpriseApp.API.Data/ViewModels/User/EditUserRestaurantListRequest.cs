using System;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Models.Common;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class EditUserRestaurantListRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string ListName { get; set; }

        [Required, MaxLength(200)]
        public string IconId { get; set; }

        [Required, MaxLength(200)]
        [RegularExpression(APIConstants.HEXCOLOR_REGEX, ErrorMessage = APIConstants.HEXCOLOR_REGEX_ERROR)]
        public string LColour { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
