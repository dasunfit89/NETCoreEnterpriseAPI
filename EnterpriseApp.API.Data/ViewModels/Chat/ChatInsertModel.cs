using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ChatInsertModel : BaseEntityInsertModel
    {
        public string Text { get; set; }

        public List<FileUploadModel> Attachments { get; set; }

        [Required]
        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        [Required]
        public string SenderName { get; set; }

        public string ReceiverName { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        [Required]
        public int ChatType { get; set; }

    }
}