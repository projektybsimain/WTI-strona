using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShareFun.Classes
{
    public class PostsTable
    {
        private SqlDatabase database;
        private const string tableName = "Posts";

        public PostsTable(SqlDatabase database)
        {
            this.database = database;
        }

        public void Add(string userID, string title, string imageName)
        {
            string commandText = "INSERT INTO " + tableName + " (AuthorID, Title, Image) VALUES (@userID, @title, @fileName)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@userID", userID },
                { "@title", title },
                { "@fileName", imageName }
            };
            database.ExecuteNonQuery(commandText, parameters);
        }

        public bool Exist(string postID)
        {
            string commandText = "SELECT * FROM Posts WHERE PostID = " + postID;
            return database.ExecuteScalar(commandText, null);
        }

        private Stack<Post> GetPosts(string commandText)
        {
            Stack<Post> posts = new Stack<Post>();
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            while (reader.Read())
            {
                string id = reader["PostID"].ToString();
                string authorId = reader["AuthorID"].ToString();
                string title = reader["Title"].ToString();
                string imagePath = reader["Image"].ToString();
                string starsCount = reader["StarsCount"].ToString();
                string date = reader["Date"].ToString();
                date = date.Substring(0, date.IndexOf(" "));
                Post post = new Post()
                {
                    Id = id,
                    AuthorID = authorId,
                    Title = title,
                    ImagePath = imagePath,
                    Stars = starsCount,
                    Date = date
                };
                posts.Push(post);
            }
            reader.Close();
            database.CloseConnection();
            return posts;
        }

        public Stack<Post> GetPendingPosts()
        {
             return GetPosts("SELECT * FROM Posts WHERE IsAccepted = 0");
        }

        public Stack<Post> GetBestPosts()
        {
            return GetPosts("SELECT * FROM Posts WHERE IsAccepted = 1 Order By StarsCount");
        }

        public Stack<Post> GetLastPosts()
        {
            return GetPosts("SELECT * FROM Posts WHERE IsAccepted = 1 Order By PostID");
        }

        public Stack<Post> GetPostsByUserID(string userID)
        {
            return GetPosts("SELECT * FROM Posts WHERE AuthorID = '" + userID + "' AND IsAccepted = 1");
        }

        public void AcceptPost(string postID)
        {
            string commandText = "UPDATE Posts SET IsAccepted = 1 WHERE PostID = '" + postID + "'";
            database.ExecuteNonQuery(commandText, null);
        }

        public void RemovePost(string postID)
        {
            string commandText = "DELETE FROM Posts WHERE PostID = '" + postID + "'";
            database.ExecuteNonQuery(commandText, null);
        }
    }
}