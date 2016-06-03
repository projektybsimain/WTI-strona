using ShareFun.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShareFun.Classes
{
    public class UserTable
    {
        private SqlDatabase database;
        private const string tableName = "Users";

        public UserTable(SqlDatabase database)
        {
            this.database = database;
        }

        public void Insert(ApplicationUser user)
        {
            string commandText = "INSERT INTO Users (UserID, Email, Login, Password) VALUES (@userID, @email, @login, @pass)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@userID", user.Id },
                { "@email", user.Email },
                { "@login", user.UserName },
                { "@pass", user.PasswordHash }
            };
            database.ExecuteNonQuery(commandText, parameters);
        }

        public bool Exist(string userName)
        {
            string commandText = "SELECT * FROM Users WHERE Login = '" + userName + "'";
            return database.ExecuteScalar(commandText, null);
        }

        public void UpdatePassword(ApplicationUser user)
        {
            string commandText = "UPDATE Users SET Password = '" + user.PasswordHash + "' WHERE UserID = '" + user.Id + "'";
            database.ExecuteNonQuery(commandText, null);
        }

        private List<ApplicationUser> GetUsers(string key, string keyValue)
        {
            string commandText = "SELECT * FROM " + tableName + " WHERE " + key + " = @key";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@key", keyValue } };

            SqlDataReader reader = database.ExecuteReader(commandText, parameters);

            List<ApplicationUser> users = new List<ApplicationUser>();

            while (reader.Read())
            {
                users.Add(new ApplicationUser()
                {
                    Id = reader["UserID"].ToString(),
                    Email = reader["Email"].ToString(),
                    UserName = reader["Login"].ToString(),
                    PasswordHash = reader["Password"].ToString()

                });
            }
            reader.Close();
            database.CloseConnection();
            return users;
        }

        public List<ApplicationUser> GetUserByName(string userName)
        {
            return GetUsers("Login", userName);
        }

        public List<ApplicationUser> GetUserByEmail(string email)
        {
            return GetUsers("Email", email);
        }

        public List<ApplicationUser> GetUserByID(string userID)
        {
            return GetUsers("UserID", userID);
        }

        public string GetPasswordHash(string userId)
        {
            List<ApplicationUser> users = GetUserByID(userId);
            if (users == null || users.Count == 0)
                return null;
            return users[0].PasswordHash;
        }

        public void SetEmailConfirmed(ApplicationUser user, bool enabled)
        {
            string bit = "0";
            if (enabled)
                bit = "1";
            string commandText = "UPDATE Users SET IsVerified = '" + bit + "' WHERE UserID = '" + user.Id + "'";
            int result = database.ExecuteNonQuery(commandText, null);
        }

        public bool IsEmailConfirmed(string userId)
        {
            string commandText = "SELECT * FROM Users WHERE UserID = '" + userId + "' AND IsVerified = 1";
            return database.RowExists(commandText);
        }

        public string GetRegistrationDate(string userId)
        {
            string commandText = "SELECT * FROM Users WHERE UserID = '" + userId + "'";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            if (reader.Read())
            {
                string date = reader["RegistrationDate"].ToString();
                return date.Substring(0, date.IndexOf(" "));
            }
            return "ERROR";
        }

        public bool CanSeePostSettings(string userId)
        {
            if (userId == null)
                return false;
            string commandText = "SELECT * FROM Users WHERE UserID = '" + userId + "'";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            if (reader.Read())
            {
                bool result = false;
                string accountType = reader["AccountType"].ToString();
                if (accountType == "1" || accountType == "2")
                    result = true;
                reader.Close();
                return result;
            }
            return false;
        }
    }
}