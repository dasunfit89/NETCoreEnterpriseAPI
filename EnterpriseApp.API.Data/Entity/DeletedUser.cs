using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Models.Entity
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string UEmail { get; set; }

        public string UName { get; set; }

        public string UFirstName { get; set; }

        public string ULastName { get; set; }

        public string USex { get; set; }

        public DateTime? DOB { get; set; }

        public string UPassword { get; set; }

        public int UFaceBookUser { get; set; }
        
        public string FBId { get; set; }

        public int? UserOtp { get; set; }

        public string FBImageURL { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<UserVisitedRestaurant> UserVisitedResturants { get; set; }

        public virtual ICollection<UserVisitedCountries> UserVisitedCountries { get; set; }

        public virtual ICollection<UserResList> ResList { get; set; }

        public virtual ICollection<ResComment> CommentsList { get; set; }

        public virtual ICollection<UserRestaurantRequest> ResRequestList { get; set; }

        public virtual ICollection<UserUType> UserTypes { get; set; }

        public virtual ICollection<UserFileUpload> Images { get; set; }

        public virtual ICollection<UserFavouriteRestaurant> UserFavouriteRestaurantList { get; set; }

        public virtual ICollection<UserReward> Rewards { get; set; }

        public virtual ICollection<UserRestaurantChoice> ChoiceRestaurantList { get; set; }
    }
}
