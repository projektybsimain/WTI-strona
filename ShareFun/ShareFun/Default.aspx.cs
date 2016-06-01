using Microsoft.AspNet.Identity.Owin;
using ShareFun.Classes;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace ShareFun
{
    public partial class _Default : Page
    {
        Stack<Post> posts;
        bool canSeePostSettings;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                    string userID = ApplicationUserStore.GetUserIDByName(manager, User);
                    string postID = Request["__EVENTARGUMENT"];
                    UpdateStarsCount(postID, userID);
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

        private void DisplayPosts()
        {
            if (posts == null)
            {
                SqlDatabase database = new SqlDatabase();
                PostsTable postsTable = new PostsTable(database);
                posts = postsTable.GetLastPosts();
                UserTable userTable = new UserTable(database);
                canSeePostSettings = userTable.CanSeePostSettings(null);
            }
            for (int i = 0; i < 20; i++)
            {
                if (posts.Count == 0)
                {
                    LoadMore.Visible = false;
                    return;
                }
                Post post = posts.Pop();
                PostsTableView.Rows.Add(post.GetRow(canSeePostSettings));
            }
        }

        protected void LoadMore_Click(object sender, EventArgs e)
        {
            DisplayPosts();
        }
    }
}