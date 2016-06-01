using Microsoft.AspNet.Identity.Owin;
using ShareFun.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;

namespace ShareFun
{
    public partial class WaitingRoom : System.Web.UI.Page
    {
        Stack<Post> posts;
        bool canSeePostSettings;
        string userID;

        protected void Page_Load(object sender, EventArgs e)
        {
            canSeePostSettings = false;
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUserManager manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                userID = ApplicationUserStore.GetUserIDByName(manager, User);
            }
            DisplayPosts();
            if (Page.IsPostBack)
            {
                string postID = Request["__EVENTARGUMENT"];
                if (Request["__EVENTTARGET"] == "starClick")
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        Response.Redirect(Page.ResolveUrl("~/Account/Login"));
                        return;
                    }
                    UpdateStarsCount(postID, userID);
                }
                else if (Request["__EVENTTARGET"] == "acceptPost")
                {
                    SqlDatabase database = new SqlDatabase();
                    PostsTable postsTable = new PostsTable(database);
                    postsTable.AcceptPost(postID);
                }
                else if (Request["__EVENTTARGET"] == "removePost")
                {
                    SqlDatabase database = new SqlDatabase();
                    PostsTable postsTable = new PostsTable(database);
                    postsTable.RemovePost(postID);
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
                posts = postsTable.GetPendingPosts();
                UserTable userTable = new UserTable(database);
                canSeePostSettings = userTable.CanSeePostSettings(userID);
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