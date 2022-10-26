using Deliciously.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deliciously.API.Data.Repo.User.Interfaces
{
    interface IUserRepo
    {
        UserModels GetUserAllUsers();
    }
}
