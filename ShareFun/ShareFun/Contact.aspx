<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ShareFun.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Contact with us!!</h3>
    <div style="width: 291px; height: 200px; float: left; width:30%;">
    <address>
       Plac Marii Skłodowskiej-Curie 5<br />
         60-965 Poznań<br />
        <abbr title="Phone">Phone:</abbr>
        648 258 467
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@example.com">support_mail@gamil.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">marketing_mail@gamil.com</a>
    </address>
    
    </div>

    <div style ="float:left; width:573px; height:200px; width:70%;">
        <center><img src="Images/telefon.gif" alt="Kontakt" style="width:201px; height:195px; "></center>
    </div> 

</asp:Content>
