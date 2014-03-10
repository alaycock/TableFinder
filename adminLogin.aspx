<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="adminLogin.aspx.cs" Inherits="_Default"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Please login using your provided confirmation code</h2>
    <div style="margin-left:370px; width:500px; position:relative; float:left;">
        <table style="border-collapse: collapse; border: none;">
            <tr>
                <td>
                    <asp:TextBox ID="usernameTextbox" runat="server" Width="200" Text="Username" onfocus="if (this.value == 'Name') this.value='';" onblur="if (this.value == '') this.value='Name';" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="passwordTextbox" runat="server" Width="200" Text="Password" onfocus="if (this.value == 'Passcode') this.value='';" onblur="if (this.value == '') this.value='Passcode';"/>
                </td>
            </tr>    
            <tr>
                <td>
                    <asp:Button ID="passwordSubmit" runat="server" OnClick="checkPassword_button" Text="Submit" />
                </td>
            </tr>
                    
        </table>
    </div>

    <div style="color: Red; width:320px; float:left; position:relative;">
        <%
            String writeValue = "<p>" + errorMessage + "</p>";
            Response.Write(writeValue);
            %>
    </div>
</asp:Content>
