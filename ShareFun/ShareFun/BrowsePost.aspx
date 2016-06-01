<%@ Page Title="Best" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BrowsePost.aspx.cs" Inherits="ShareFun.BrowsePost" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<style>
#header {
    background-color:#F0F0F0;
    padding:1px;
}
h3 {
	display: inline-block;
}
#image {
  display: block;
  margin-left: auto;
  margin-right: auto;
  margin-top:10px;
}
img.star {
vertical-align:middle;
  margin-bottom: 5px;
  margin-left: 5px;
  margin-right: 5px;
}
#section {
margin-bottom: 25px;
}
#postdate {
color: #808080;
display: inline;
margin-left: 8px;
}
#MainContent_CommentText {
display: inline-block;
}
#MainContent_PostComment {
margin-left: 8px;
}
</style>
    </br>
    <div id="postContent" runat="server"></div>
    <h3 id="commentsLabel" runat="server">Comments:</h3></br>
    <div id="showcommentsform" runat="server" visible="false">
    <asp:TextBox ID="CommentText" runat="server" Width="900px" CssClass="form-control"></asp:TextBox><asp:Button ID="PostComment" runat="server" Text="Write comment" CssClass="btn btn-primary" OnClick="PostComment_Click" />
    </div>
    <asp:Table ID="CommentsTable" runat="server"></asp:Table>
</asp:Content>
