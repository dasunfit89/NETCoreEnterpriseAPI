using System;
using System.Collections.Generic;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UserCommentModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserCommentModel()
        {

        }
    }
}
