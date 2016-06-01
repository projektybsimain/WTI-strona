using System.Web.UI.WebControls;

namespace ShareFun.Classes
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Stars { get; set; }

        public TableRow GetRow(bool settingsVisible)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "<div id=\"header\"><a href=\"#\" onclick=\"__doPostBack('starClick', '" + Id + "')\"><img class=\"star\" src=\"media/star.png\" width=\"50\" /></a><a href=\"BrowsePost?id=" + Id + "\"><h3>" + Title + " (" + Stars + ")</h3></a></div><div id=\"section\"><img id=\"image\" src=\"" + ImagePath + "\" width=\"500\" /></div>";
            if (settingsVisible)
                cell.Text += "<div class=\"form-group\"><button onclick=\"__doPostBack('acceptPost', '" + Id + "')\" class=\"btn btn-success\" type=\"button\">Accept</button><button onclick=\"__doPostBack('removePost', '" + Id + "')\" class=\"btn btn-danger\" type=\"button\">Remove</button></div>";
            row.Cells.Add(cell);
            return row;
        }
    }
}