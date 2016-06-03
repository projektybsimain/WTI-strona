<%@ Page Title="Add new post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewPost.aspx.cs" Inherits="ShareFun.NewPost" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <div>
        <asp:Label ID="ErrorMessageText" Visible="false" runat="server" Text="NULL" CssClass="text-danger"></asp:Label>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <asp:Label ID="SuccessMessageText" runat="server" Text="NULL" CssClass="text-success"></asp:Label><a href="/WaitingRoom" class="text-info">Waiting Room</a>
        </asp:PlaceHolder>
    </div>
    </br>
    <asp:Label ID="Label1" runat="server" Text="Title:" AssociatedControlID="TitleTextBox" CssClass="col-md-2 control-label" Font-Bold="True"></asp:Label>
        <asp:TextBox ID="TitleTextBox" runat="server" MaxLength="64" CssClass="form-control" ControlToValidate="TitleTextBox"></asp:TextBox>
        </br>
        <asp:Label ID="Label2" runat="server" Text="Image preview:" CssClass="col-md-2 control-label" Font-Bold="True"></asp:Label>

        <div id="dvPreview">
            <img src="Images/image-icon.png" width="500"/>
        </div>
        </br>
        <input id="fileupload" type="file" runat="server" ClientIDMode="Static"/>
        </br>
    <div class="col-md-offset-2 col-md-10">
        <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" CssClass="btn btn-primary" />
        <asp:Button ID="Cancel" runat="server" Text="Cancel" OnClick="Cancel_Click" CssClass="btn btn-default" />
    </div>

<script type="text/javascript" src="/Scripts/jquery-1.8.3.min.js"></script>
<script language="javascript" type="text/javascript">
$(function () {
    $("#fileupload").change(function () {
        $("#dvPreview").html("");
        var regex = /^.+(.jpg|.jpeg|.gif|.png)$/;
        if (regex.test($(this).val().toLowerCase())) {
            if ($.browser.msie && parseFloat(jQuery.browser.version) <= 9.0) {
                $("#dvPreview").show();
                $("#dvPreview")[0].filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = $(this).val();
            }
            else {
                if (typeof (FileReader) != "undefined") {
                    $("#dvPreview").show();
                    $("#dvPreview").append("<img />");
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("#dvPreview img").attr("src", e.target.result);
                        $("#dvPreview img").attr("width", 500);
                    }
                    reader.readAsDataURL($(this)[0].files[0]);
                } else {
                    alert("This browser does not support FileReader.");
                }
            }
        } else {
            alert("Please upload a valid image file.");
        }
    });
});
</script>

</asp:Content>
