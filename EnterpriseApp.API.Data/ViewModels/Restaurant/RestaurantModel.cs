using EnterpriseApp.API.Models.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class RestaurantModel
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

        public bool Session1 { get; set; }

        public bool Session2 { get; set; }

        public List<CommonFileUploadModel> Images { get; set; }

        public List<ResCommentModel> Comments { get; set; }

        public List<UserFavouriteRestaurantModel> UserFavouriteRestaurantList { get; set; }

        public List<ResBadgeModel> ResBadges { get; set; }

        public List<ResBadgeModel> RestaurantBadges { get; set; }

        public List<UserVisitedRestaurantModel> VisitedUsers { get; set; }

        public List<ResFilterIconModel> Icons { get; set; }

        public List<ResOpeningDayModel> OpeningHours { get; set; }

        public List<EditResOpeningDayModel> OpeningHoursList { get; set; }

        public List<SelectListItem> SubcategoriesList { get; set; }

        public double Distance { get; set; }

        public bool IsOpen { get; set; }

        public RestaurantModel()
        {
        }

        public string otherTime { get; set; }
    }

    public class DeleteRestaurent
    {
        public bool IsSuccessful { get; set; }
    }

    public class RestaurantResponse
    {
        public int Id { get; set; }
        public string RLatitude { get; set; }
        public string RLongitude { get; set; }
        public string RDescription { get; set; }
        public string RMetro { get; set; }
        public string MapIcon { get; set; }
        public string RName { get; set; }
        public double Distance { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFavourite { get; set; }
        public List<ResBadgeModel> RestaurantBadges { get; set; }
        public IEnumerable<CommonFileUploadModel> Images { get; set; }
    }

    public class RestaurantModelResponse
    {
        public List<BackOfficeRestaurant> List { get; set; }

        public int TotalRecords { get; set; }

        public RestaurantModelResponse()
        {
            List = new List<BackOfficeRestaurant>();

        }
    }

    public class BackOfficeRestaurant
    {
        public int Id { get; set; }

        public string RName { get; set; }

        public string RAddress { get; set; }

        public string RMetro { get; set; }

        public string MapIcon { get; set; }

        public bool Status { get; set; }
    }

}
