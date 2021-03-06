﻿using System.Web.UI.WebControls;

namespace ShareFun.Classes
{
    public class Comment
    {
        public string Author { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }

        public TableRow GetRow()
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            string date = Date.Substring(0, Date.IndexOf(" "));
            cell.Text = "<h2>" + Text + "</h2><b>" + Author + "</b><p id=\"postdate\">" + date + "</p>";
            row.Cells.Add(cell);
            return row;
        }
    }
}