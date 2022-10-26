using System.Collections.Generic;
using EnterpriseApp.API.Models.ViewModels.Restaurant;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class RestaurantModelBackOffice
    {
        public int Id { get; set; }

        public string RName { get; set; }

        public string RStreet { get; set; }

        public string RCity { get; set; }

        public string RDescription { get; set; }

        public string RLongitude { get; set; }

        public string RLatitude { get; set; }

        public string RPostalCode { get; set; }

        public string RContact { get; set; }

        public string MapIcon { get; set; }

        public string RPriceDes { get; set; }

        public string RPrice { get; set; }

        public string RAddress { get; set; }

        public string RMetro { get; set; }

        public string RRecommendation { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string GooglePlaceId { get; set; }

        public string RClose { get; set; }

        public string Rate { get; set; }

        public int Status { get; set; }

        public string MyChoice { get; set; }

        public List<CommonFileUploadModel> Images { get; set; }

        public List<ResCommentModel> Comments { get; set; }

        public List<UserFavouriteRestaurantModel> UserFavouriteRestaurantList { get; set; }

        public List<ResBadgeModel> ResBadge { get; set; }

        public List<UserVisitedRestaurantModel> VisitedUsers { get; set; }

        public List<ResFilterIconModel> Icons { get; set; }

        public List<ResOpeningDayModel> OpeningHours { get; set; }

        public List<EditResOpeningDayModel> OpeningHoursList { get; set; }
         
        public double Distance { get; set; }

        public bool IsOpen { get; set; }

        public RestaurantModelBackOffice()
        {
        }
    }
}
