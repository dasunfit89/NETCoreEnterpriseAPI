using EnterpriseApp.API.Models.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class RestaurantUpdateRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }

        [Required, MaxLength(200)]
        [DisplayName("Name")]
        public string RName { get; set; }

        [MaxLength(200)]
        [DisplayName("Street")]
        public string RStreet { get; set; }

        [Required, MaxLength(200)]
        [DisplayName("City")]
        public string RCity { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [DisplayName("Countries")]
        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required, MaxLength(200)]
        [DisplayName("Description")]
        public string RDescription { get; set; }

        [Required, MaxLength(200)]
        [DisplayName("Longitude")]
        public string RLongitude { get; set; }

        [Required, MaxLength(200)]
        [DisplayName("Latitude")]
        public string RLatitude { get; set; }

        [Required, MaxLength(200)]
        [DisplayName("Google PlaceId")]
        public string GooglePlaceId { get; set; }

        [DisplayName("Close")]
        public string RClose { get; set; }

        [DisplayName("Postal Code")]
        public string RPostalCode { get; set; }

        [MaxLength(200)]
        [DisplayName("Contact")]
        public string RContact { get; set; }

        [DisplayName("MapIcon")]
        public string MapIcon { get; set; }

        [DisplayName("PriceDes")]
        public string RPriceDes { get; set; }

        [DisplayName("Price")]
        public string RPrice { get; set; }

        [DisplayName("Address")]
        public string RAddress { get; set; }

        [DisplayName("RMetro")]
        public string RMetro { get; set; }

        [DisplayName("Recommendation")]
        public string RRecommendation { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [DisplayName("Sub-Categories")]
        public List<int> Subcategories { get; set; }

        public bool Session1 { get; set; }

        public bool Session2 { get; set; }

        [DisplayName("Category")]
        public IEnumerable<SelectListItem> Category { get; set; }

        public List<ResOpeningDayModel> OpeningHours { get; set; }

        public List<EditResOpeningDayModel> OpeningHoursList { get; set; }

        public List<CommonFileUploadModel> ImagesList { get; set; }

        public List<SelectListItem> MapIcons { get; set; }

        public List<SelectListItem> SubcategoriesList { get; set; }

        public List<SelectListItem> Days { get; set; }

        public virtual IEnumerable<ResOpeningHourModel> ROpens { get; set; }

        public List<ResBadgeModel> ResBadges { get; set; }

        public List<int> ResBadgeList { get; set; }

        public RestaurantUpdateRequest()
        {
            ResBadgeList = new List<int>();
            ResBadges = new List<ResBadgeModel>();
            OpeningHours = new List<ResOpeningDayModel>();
            OpeningHoursList = new List<EditResOpeningDayModel>();
            ImagesList = new List<CommonFileUploadModel>();
        }
    }
}
