using System;
using System.Collections.Generic;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ChatModel : BaseEntityModel
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public List<FileUploadModel> Attachments { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }

        public int DeliveryStatus { get; set; }

        public DateTime SendDate { get; set; }

        public string ThumbUrl { get; set; }

        public int ChatType { get; set; }
    }
}