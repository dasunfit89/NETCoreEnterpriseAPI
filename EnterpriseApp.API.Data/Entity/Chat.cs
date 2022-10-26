using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Chat : BaseEntity
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("attachments")]
        public List<FileUpload> Attachments { get; set; }

        [BsonElement("senderId")]
        public string SenderId { get; set; }

        [BsonElement("receiverId")]
        public string ReceiverId { get; set; }

        [BsonElement("senderName")]
        public string SenderName { get; set; }

        [BsonElement("receiverName")]
        public string ReceiverName { get; set; }

        [BsonElement("DeliveryStatus")]
        public int DeliveryStatus { get; set; }

        [BsonElement("sendDate")]
        public DateTime SendDate { get; set; }

        [BsonElement("chatType")]
        public int ChatType { get; set; }

        [BsonElement("thumbUrl")]
        public string ThumbUrl { get; set; }

        public Chat()
        {
            Attachments = new List<FileUpload>();
        }
    }
}
