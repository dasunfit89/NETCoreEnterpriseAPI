using System;
namespace EnterpriseApp.API.Models.ViewModels
{
    public class GetRestaurantListRequest : PagingQueryParam
    {
        public double User_Location_Lat { get; set; }

        public double User_Location_Lon { get; set; }

        public string UserId { get; set; }
    }
}
