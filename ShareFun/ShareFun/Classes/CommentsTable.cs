using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShareFun.Classes
{
    public class CommentsTable
    {
        private SqlDatabase database;

        public CommentsTable(SqlDatabase database)
        {
            this.database = database;
        }

        public void Insert(string postID, string authorID, string text)
        {
            string commandText = "INSERT INTO Comments (PostID, AuthorID, Text) VALUES ('" + postID + "', '" + authorID + "', '" + text + "')";
            database.ExecuteNonQuery(commandText, null);
        }

        public Stack<Comment> GetComments(string postID)
        {
            Stack<Comment> comments = new Stack<Comment>();
            Queue<Comment> queue = new Queue<Comment>();
            string commandText = "SELECT * FROM Comments WHERE PostID = '" + postID + "'";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            UserTable userTable = new UserTable(database);
            while (reader.Read())
            {
                string author = reader["AuthorID"].ToString();
                string text = reader["Text"].ToString();
                string date = reader["Date"].ToString();
                Comment comment = new Comment()
                {
                    Author = author,
                    Text = text,
                    Date = date
                };
                queue.Enqueue(comment);
            }
            reader.Close();
            database.CloseConnection();
            while (queue.Count > 0)
            {
                Comment comment = queue.Dequeue();
                comment.Author = userTable.GetUserByID(comment.Author)[0].UserName;
                comments.Push(comment);
            }
            return comments;
        }
    }
}