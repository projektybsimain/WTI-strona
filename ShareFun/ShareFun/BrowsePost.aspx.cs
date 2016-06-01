using Microsoft.AspNet.Identity.Owin;
using ShareFun.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ShareFun
{
    public partial class BrowsePost : System.Web.UI.Page
    {
        Stack<Comment> comments;
        string postID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                showcommentsform.Visible = true;

            postID = Request.QueryString["id"];
            SqlDatabase database = new SqlDatabase();
            PostsTable posts = new PostsTable(database);
            if (String.IsNullOrEmpty(postID) || !posts.Exist(postID))
            {
                Response.Redirect(Page.ResolveUrl("~/"));
            }
            string commandText = "SELECT * FROM Posts WHERE PostID = '" + postID + "'";
            SqlDataReader reader = database.ExecuteReader(commandText, null);
            reader.Read();
            string postTitle = reader["Title"].ToString();
            string starsCount = reader["StarsCount"].ToString();
            string imagePath = reader["Image"].ToString();
            reader.Close();
            database.CloseConnection();
            postContent.InnerHtml = "<div id=\"header\"><a href=\"#\" onclick=\"__doPostBack('starClick', '" + postID + "')\"><img class=\"star\" src=\"media/star.png\" width=\"50\" /></a><h3>" + postTitle + " (" + starsCount + ")</h3></div><div id=\"section\"><img id=\"image\" src=\"" + imagePath + "\" width=\"500\" /></div>";
            DisplayComments();

            if (Page.IsPostBack)
            {
                if (Request["__EVENTTARGET"] == "starClick")
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        Response.Redirect(Page.ResolveUrl("~/Account/Login"));
                        return;
                    }
                    ApplicationUserManager manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    string userID = ApplicationUserStore.GetUserIDByName(manager, User);
                    string _postID = Request["__EVENTARGUMENT"];
                    UpdateStarsCount(_postID, userID);
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        private void UpdateStarsCount(string postID, string userID)
        {
            SqlDatabase database = new SqlDatabase();
            StarsLogTable starsLogTable = new StarsLogTable(database);
            if (starsLogTable.AlreadyLogged(postID, userID))
                starsLogTable.DeleteLog(postID, userID);
            else
                starsLogTable.AddLog(postID, userID);
        }

        private void DisplayComments()
        {
            if (comments == null)
            {
                SqlDatabase database = new SqlDatabase();
                CommentsTable commentsTable = new CommentsTable(database);
                comments = commentsTable.GetComments(postID);
            }
            if (!User.Identity.IsAuthenticated && comments.Count == 0)
            {
                commentsLabel.Visible = false;
            }
            while (comments.Count > 0)
            {
                Comment comment = comments.Pop();
                CommentsTable.Rows.Add(comment.GetRow());
            }
        }

        protected void PostComment_Click(object sender, EventArgs e)
        {
            string text = CommentText.Text;
            if (String.IsNullOrEmpty(text))
                return;
            SqlDatabase database = new SqlDatabase();
            CommentsTable comments = new CommentsTable(database);
            ApplicationUserManager manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string userID = ApplicationUserStore.GetUserIDByName(manager, User);
            comments.Insert(postID, userID, text);
            Response.Redirect(Request.RawUrl);
        }
    }
}