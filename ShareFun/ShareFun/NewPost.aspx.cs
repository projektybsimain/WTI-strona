using Microsoft.AspNet.Identity.Owin;
using ShareFun.Classes;
using ShareFun.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace ShareFun
{
    public partial class NewPost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect(Page.ResolveUrl("~/Account/Login"));
                return;
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            ApplicationUserManager manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string userID = ApplicationUserStore.GetUserIDByName(manager, User);
            SqlDatabase sqlDatabase = new SqlDatabase();
            UserTable userTable = new UserTable(sqlDatabase);

            string title = TitleTextBox.Text.Trim();
            string fileName = fileupload.PostedFile.FileName.Trim();
            if (String.IsNullOrEmpty(title))
            {
                ShowError("The title field is required!");
                return;
            }
            if (String.IsNullOrEmpty(fileName))
            {
                ShowError("The image is required!");
                return;
            }
            if (fileupload.PostedFile.ContentLength > 2097152)
            {
                ShowError("The maximum file size is 2 MB!");
                return;
            }
            if (fileName.Length > 64)
            {
                ShowError("The maximum file name length is 64!");
                return;
            }
            if (!fileName.EndsWith(".jpg") && !fileName.EndsWith(".png") && !fileName.EndsWith(".gif") && !fileName.EndsWith(".jpeg"))
            {
                ShowError("Only jpg, png and gif files are supported!");
                return;
            }
            PostsTable posts = new PostsTable(sqlDatabase);

            DateTime date = DateTime.Now;
            string fileExtension = Path.GetExtension(fileName);
            string directoryPath = Server.MapPath("/") + "media\\" + date.Year + "\\" + date.Month;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            fileName = directoryPath + "\\" + User.Identity.Name + GetRandomString() + fileExtension;
            string fileUrl = MapURL(Page.ResolveUrl(fileName));
            posts.Add(userID, title, fileUrl);
            fileupload.PostedFile.SaveAs(fileName);
            sqlDatabase.CloseConnection();
            ErrorMessageText.Visible = false;
            SuccessMessageText.Text = String.Format("Your post \"{0}\" has been uploaded successfully and is waiting for approval in ", title);
            successMessage.Visible = true;
        }

        private string MapURL(string path)
        {
            string appPath = Server.MapPath("/").ToLower();
            return string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
        }

        private string GetRandomString()
        {
            string stringBase = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789";
            string random = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                random += stringBase[rand.Next(stringBase.Length)];
            }
            return random;
        }

        private void ShowError(string text)
        {
            ErrorMessageText.Visible = true;
            ErrorMessageText.Text = text;
        }
    }
}