using Microsoft.AspNet.Identity.Owin;
using ShareFun.Classes;
using ShareFun.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace ShareFun
{
    public partial class BrowseUser : System.Web.UI.Page
    {
        Stack<Post> posts;
        string userName = null;
        string userID = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            userName = Request.QueryString["id"];
            SqlDatabase database = new SqlDatabase();
            UserTable users = new UserTable(database);
            if (String.IsNullOrEmpty(userName) || !users.Exist(userName))
            {
                Response.Redirect(Page.ResolveUrl("~/"));
            }
            ApplicationUserStore userStore = new ApplicationUserStore();
            Task<ApplicationUser> userTask = userStore.FindByNameAsync(userName);
            userID = userTask.Result.Id;
            string date = users.GetRegistrationDate(userID);

            NameLabel.Text = "Name: " + userName;
            RegisteredLabel.Text = "Registration date: " + date;

            DisplayPosts();
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
                    string _userID = ApplicationUserStore.GetUserIDByName(manager, User);
                    string postID = Request["__EVENTARGUMENT"];
                    UpdateStarsCount(postID, _userID);
                }
            }
            database.CloseConnection();
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

        private void DisplayPosts()
        {
            if (posts == null)
            {
                SqlDatabase database = new SqlDatabase();
                PostsTable postsTable = new PostsTable(database);
                posts = postsTable.GetPostsByUserID(userID);
                UserTable userTable = new UserTable(database);
                if (posts.Count > 0)
                {
                    UserPostsLabel.Text = "Posts added by " + userName + ":";
                    UserPostsLabel.Visible = true;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (posts.Count == 0)
                {
                    LoadMore.Visible = false;
                    return;
                }
                Post post = posts.Pop();
                PostsTableView.Rows.Add(post.GetRow(false));
            }
        }

        protected void LoadMore_Click(object sender, EventArgs e)
        {
            DisplayPosts();
        }
    }
}