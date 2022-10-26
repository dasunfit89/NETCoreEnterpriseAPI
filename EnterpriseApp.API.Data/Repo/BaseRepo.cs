using System;
using System.Collections.Generic;
using System.Text;

namespace Deliciously.API.Data.Repo
{
    public class BaseRepo
    {
        public BaseRepo()
        {
            var conn = GetConnection();
        }
    }
}
