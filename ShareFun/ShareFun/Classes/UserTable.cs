using Microsoft.AspNet.Identity.EntityFramework;
using ShareFun.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ShareFun.Classes
{
    public class UserTable
    {
        private SqlDatabase database;

        public UserTable(SqlDatabase database)
        {
            this.database = database;
        }

        /*public bool AuthenticateUser(string login, string passwordHash)
        {
            var connection = Connect();
            if (connection == null)
                return false;
            string commandText = "SELECT TOP 1 * FROM Users WHERE Login = '" + login + "' AND Password = '" + passwordHash + "'";
            SqlCommand command = new SqlCommand(commandText, connection);
            return command.ExecuteScalar() != null;
        }*/

        public void Insert(ApplicationUser user)
        {
            string commandText = "INSERT INTO Users (Email, Login, Password) VALUES (@email, @login, @pass)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@email", user.Email },
                { "@login", user.UserName },
                { "@pass", user.PasswordHash }
            };
            database.ExecuteNonQuery(commandText, parameters);
        }

        private List<ApplicationUser> GetUsers(string tableName, string key, string keyValue)
        {
            string commandText = "SELECT * FROM " + tableName + " WHERE " + key + " = @key";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@key", keyValue } };

            SqlDataReader reader = database.ExecuteReader(commandText, parameters);

            List<ApplicationUser> users = new List<ApplicationUser>();

            while (reader.Read())
            {
                users.Add(new ApplicationUser() { UserName = reader[key].ToString() });
            }
            reader.Close();
            database.CloseConnection();
            return users;
        }

        public List<ApplicationUser> GetUserByName(string userName)
        {
            return GetUsers("Users", "Login", userName);
        }

        public List<ApplicationUser> GetUserByEmail(string email)
        {
            return GetUsers("Users", "Email", email);
        }
    }
}