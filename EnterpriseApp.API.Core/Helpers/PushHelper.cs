using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace EnterpriseApp.API.Core.Helpers
{
    public static class PushHelper
    {
        public static async Task<bool> SendPush(PushNotificationRequest request)
        {
            bool isSuccess = true;

            try
            {
                if (FirebaseMessaging.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("service_account.json"),
                    });
                }

                foreach (var device in request.Devices)
                {
                    var message = new FirebaseAdmin.Messaging.Message()
                    {
                        Token = device.PushToken,
                        Data = new Dictionary<string, string>()
                        {
                            { "title", request.Title},
                            { "body", request.Body },
                            { "messageType", request.MessageType },
                            { "messageParam",  request.MessageParam }

                        },
                        //Notification = new Notification()
                        //{
                        //    Title = request.Title,
                        //    Body = request.Body,
                        //},
                        Apns = new ApnsConfig()
                        {
                            Headers = new Dictionary<string, string>()
                            {
                                { "apns-priority", "5"},
                                { "apns-push-type", "background"},
                                { "apns-topic","nl.procura.stembureau-app"},
                            },
                            Aps = new Aps() { ContentAvailable = true, },
                        },
                        Android = new AndroidConfig()
                        {
                            Priority = Priority.High,
                        },
                    };

                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                }
            }
            catch (Exception ex)
            {
                isSuccess = false;

                Console.WriteLine(ex);
            }

            return isSuccess;
        }
    }
}
