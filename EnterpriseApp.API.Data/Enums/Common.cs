using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Data
{
    public enum WellKnownStatus
    {
        Deactive = 0,
        Active = 1,
        Pending = 2,
        Suspended = 3,
        Deleted = 4
    }

    public enum WellKnownUserType
    {
        User = 1,
        Admin = 2,
    }

    public class APIConstants
    {
        public const string EMAIL_REGEX = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public const string EMAIL_REGEX_ERROR = "Invalid Email Address";

        public const string HEXCOLOR_REGEX = @"^#(?:[0-9a-fA-F]{3}){1,2}$";

        public const string HEXCOLOR_REGEX_ERROR = "Invalid Hex Color";

        public const string PHONE_REGEX = @"^[0-9]{10}$";

        public const string PHONE_REGEX_ERROR = "Invalid Phone Number";

        public const string TITLE_ERROR = "Title must be either  'Not Set', 'Rev', 'Mrs', 'Miss', 'Rev', 'Dr', 'Mr'.";

        public const string GENDER_ERROR = "Gender must be either 'Not Set', 'Male', 'Female' or 'Other'.";
    }

    public enum WellKnownFileUploadType
    {
        None = 0,
        Article = 1,
        User = 2
    }

    public enum WellknownChatDeliveryStatus
    {
        QUEUED = 1,
        SENT = 2,
        RECEIVED = 3,
        DELIVERED = 4,
        READ = 5,
    }
}
