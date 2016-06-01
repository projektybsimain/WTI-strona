using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShareFun.Classes
{
    public class StarsLogTable
    {
        private SqlDatabase database;
        private const string tableName = "StarsLog";

        public StarsLogTable(SqlDatabase database)
        {
            this.database = database;
        }

        public bool AlreadyLogged(string postID, string userID)
        {
            string commandText = "SELECT TOP 1 * FROM " + tableName + " WHERE PostID = '" + postID + "' AND AssessorID = '" + userID + "'";
            bool result = database.ExecuteScalar(commandText, null);
            return result;
        }

        public void DeleteLog(string postID, string userID)
        {
            string commandText = "DELETE FROM " + tableName + " WHERE PostID = '" + postID + "' AND AssessorID = '" + userID + "'";
            database.ExecuteNonQuery(commandText, null);
        }

        public void AddLog(string postID, string userID)
        {
            string commandText = "INSERT INTO " + tableName + " (PostID, AssessorID) VALUES (@postID, @userID)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@postID", postID },
                { "@userID", userID }
            };
            database.ExecuteNonQuery(commandText, parameters);
        }
    }
}