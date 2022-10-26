using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UpdateDeviceTokenRequest
    {
        [Required]
        public string PushToken { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
