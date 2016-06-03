using ShareFun.Models;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ShareFun.Classes
{
    public class Post
    {
        public string Id { get; set; }
        public string AuthorID { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Stars { get; set; }
        public string Date { get; set; }

        public TableRow GetRow(bool settingsVisible)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "<div id=\"header\"><a href=\"#\" onclick=\"__doPostBack('starClick', '" + Id + "')\"><img class=\"star\" src=\"media/star.png\" width=\"50\" /></a><a href=\"BrowsePost?id=" + Id + "\"><h3>" + Title + " (" + Stars + ")</h3></a></div><div id=\"section\"><img id=\"image\" src=\"" + ImagePath + "\" width=\"500\" /></div>";
            ApplicationUserStore userStore = new ApplicationUserStore();
            Task<ApplicationUser> userTask = userStore.FindByIdAsync(AuthorID);
            string userName = userTask.Result.UserName;
            cell.Text += "<p class=\"text-muted\">Author: <a class=\"text-muted\" href=\"BrowseUser?id=" + userName + "\">" + userName + "</p></a><p class=\"text-muted\">Date: " + Date + "</p>";
            if (settingsVisible)
                cell.Text += "<div class=\"form-group\"><button onclick=\"__doPostBack('acceptPost', '" + Id + "')\" class=\"btn btn-success\" type=\"button\">Accept</button><button onclick=\"__doPostBack('removePost', '" + Id + "')\" class=\"btn btn-danger\" type=\"button\">Remove</button></div>";
            row.Cells.Add(cell);
            return row;
        }
    }
}