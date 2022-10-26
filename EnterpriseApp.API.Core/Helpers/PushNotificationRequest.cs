using System;
using System.Collections.Generic;
using EnterpriseApp.API.Data.Entity;

namespace EnterpriseApp.API.Core.Helpers
{
    public class PushNotificationRequest
    {
        public List<UserDevice> Devices { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string MessageType { get; set; }

        public string MessageParam { get; set; }
    }
}
