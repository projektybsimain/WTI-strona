<%@ Page Title="Waiting Room" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WaitingRoom.aspx.cs" MaintainScrollPositionOnPostBack="true" Inherits="ShareFun.WaitingRoom" %>

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
</style> 

    <h2><%: Title %></h2>
    <asp:Table ID="PostsTableView" runat="server" Width="600" CssClass=""></asp:Table>
    <asp:Button ID="LoadMore" CssClass="btn btn-primary" runat="server" Text="Load more posts" OnClick="LoadMore_Click" />
</asp:Content>
