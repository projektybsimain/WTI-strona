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

        public Stack<Post> GetPendingPosts()
        {
            Stack<Post> posts = new Stack<Post>();
            string commandText = "SELECT * FROM Posts WHERE IsAccepted = 0";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            while (reader.Read())
            {
                string id = reader["PostID"].ToString();
                string title = reader["Title"].ToString();
                string imagePath = reader["Image"].ToString();
                string starsCount = reader["StarsCount"].ToString();
                Post post = new Post()
                {
                    Id = id,
                    Title = title,
                    ImagePath = imagePath,
                    Stars = starsCount
                };
                posts.Push(post);
            }
            reader.Close();
            database.CloseConnection();
            return posts;
        }

        public Stack<Post> GetBestPosts()
        {
            Stack<Post> posts = new Stack<Post>();
            string commandText = "SELECT * FROM Posts WHERE IsAccepted = 1 Order By StarsCount";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            while (reader.Read())
            {
                string id = reader["PostID"].ToString();
                string title = reader["Title"].ToString();
                string imagePath = reader["Image"].ToString();
                string starsCount = reader["StarsCount"].ToString();
                Post post = new Post()
                {
                    Id = id,
                    Title = title,
                    ImagePath = imagePath,
                    Stars = starsCount
                };
                posts.Push(post);
            }
            reader.Close();
            database.CloseConnection();
            return posts;
        }

        public Stack<Post> GetLastPosts()
        {
            Stack<Post> posts = new Stack<Post>();
            string commandText = "SELECT * FROM Posts WHERE IsAccepted = 1 Order By PostID";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            while (reader.Read())
            {
                string id = reader["PostID"].ToString();
                string title = reader["Title"].ToString();
                string imagePath = reader["Image"].ToString();
                string starsCount = reader["StarsCount"].ToString();
                Post post = new Post()
                {
                    Id = id,
                    Title = title,
                    ImagePath = imagePath,
                    Stars = starsCount
                };
                posts.Push(post);
            }
            reader.Close();
            database.CloseConnection();
            return posts;
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