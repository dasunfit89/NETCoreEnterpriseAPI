using EnterpriseApp.API.Models.Common;
using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class Restaurant : BaseEntity
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

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

        public string PlaceId { get; set; }

        public int CountryId { get; set; }

        public Boolean Session1 { get; set; }

        public Boolean Session2 { get; set; }
               
        public virtual Country Country { get; set; }

        public virtual ResCategory ResCategory { get; set; }

        public virtual ICollection<ResFileUpload> Images { get; set; }

        public virtual ICollection<ResComment> Comments { get; set; }

        public virtual ICollection<UserFavouriteRestaurant> UserFavouriteRestaurantList { get; set; }

        public virtual ICollection<ResBadgeEntity> ResBadgeEntity { get; set; }

        public virtual ICollection<UserVisitedRestaurant> VisitedUsers { get; set; }

        public virtual ICollection<ResFilterIcon> Icons { get; set; }

        public virtual ICollection<ResOpeningHour> OpeningHours { get; set; }

        public virtual ICollection<UserRestaurantChoice> ChoiceRestaurantList { get; set; }

        public virtual ICollection<ResListRestaurant> ResListItems { get; set; }

        public virtual ICollection<ResSubcategoryEntity> ResSubcategoryEntityList { get; set; }

        public Restaurant()
        {
            Status = (int)WellKnownStatus.Active;
        }
    }
}
