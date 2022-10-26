using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ConfigurationModel : BaseEntityModel
    {
        public bool EnableChat { get; set; }

        public string HomeCaption { get; set; }

        public string HomeDescription { get; set; }

        public string FooterCaption { get; set; }

        public string FooterDescription { get; set; }

        public string HomeCaptionEn { get; set; }

        public string HomeDescriptionEn { get; set; }

        public string FooterCaptionEn { get; set; }

        public string FooterDescriptionEn { get; set; }

        public List<FileUploadModel> Images { get; set; }

        public UserModel User { get; set; }
    }
}