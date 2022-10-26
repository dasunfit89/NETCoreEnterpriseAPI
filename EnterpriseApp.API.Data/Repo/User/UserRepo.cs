using Deliciously.API.Data.Repo.User.Interfaces;
using Deliciously.API.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Deliciously.API.Data.Repo.User
{
    public class UserRepo : BaseRepo, IUserRepo
    {
        public UserRepo()
        {
        }

        public UserModels GetUserAllUsers()
        {
            List<UserModels> users = new List<UserModels>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserModels()
                        {
                            UEmail = reader["UEmail"].ToString(),
                            UFirstName = reader["UFirstName"].ToString(),
                            UName = reader["UName"].ToString(),
                            UNationality = reader["UNationality"].ToString(),
                            UPassword = reader["UPassword"].ToString(),
                            USex = reader["USex"].ToString()
                        });
                    }
                }
            }
            throw new System.NotImplementedException();
        }

    }
}
