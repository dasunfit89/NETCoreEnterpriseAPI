using System;
namespace EnterpriseApp.API.Core.Constants
{
    public static class PushNotificationKeys
    {
        public const string REFRESH_SCREEN = "1";

        public const string PUBLIC_NOTIFICATION = "2";

        public const string CHAT_NOTIFICATION = "3";

        public const string RELOAD_CONFIG = "4";

        public const string CLEAR_CHAT_MESSAGES = "5";

        public const string FORCE_USER_LOGOUT = "6";

        public const string TEST_FCM = "0";

    }

    public static class PushNotificationStatuses
    {
        public const string MESSAGE_NEW = "NEW";

        public const string MESSAGE_RECEIVED = "RECEIVED";

        public const string MESSAGE_READ = "READ";

        public const string MESSAGE_DELIVERED = "DELIVERED";
    }

    public class PushNotificationConstants
    {
        public const string CHANNEL_ID = "fcm_default_channel";

        public const string CHANNEL_NAME = "EnterpriseApp App";
    }
}
