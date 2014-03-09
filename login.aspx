<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="login.aspx.cs" Inherits="_Default"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Please login using your provided confirmation code</h2>
    <div style="margin-left:370px; width:500px; position:relative; float:left;">
        <table style="border-collapse: collapse; border: none;">
            <tr>
                <td>
                    <asp:TextBox ID="nameTextbox" runat="server" Width="200" Text="Name" onfocus="if (this.value == 'Name') this.value='';" onblur="if (this.value == '') this.value='Name';" />
                </td>
                <td class='error'>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="Name" ErrorMessage="Please enter your name" ControlToValidate="nameTextbox" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="emailTextbox" runat="server" Width="200" Text="Email" onfocus="if (this.value == 'Email') this.value='';" onblur="if (this.value == '') this.value='Email';"/>
                </td>
                <td class='error'>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="emailTextbox" 
                        ValidationExpression="^[a-z0-9][a-z0-9_\.-]{0,}[a-z0-9]@[a-z0-9][a-z0-9_\.-]{0,}[a-z0-9][\.][a-z0-9]{2,4}$" ErrorMessage="Invalid email address"  />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="phoneTextbox" runat="server" Width="200" Text="Phone Number" onfocus="if (this.value == 'Phone Number') this.value='';" onblur="if (this.value == '') this.value='Phone Number';" />
                </td>
                <td class='error'>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        InitialValue="Phone Number" ErrorMessage="Please enter your phone number" ControlToValidate="phoneTextbox" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="schoolTextbox" runat="server" Width="200" Text="School" onfocus="if (this.value == 'School') this.value='';" onblur="if (this.value == '') this.value='School';" />
                </td>
                <td class='error'>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        InitialValue="School" ErrorMessage="Please enter your school name" ControlToValidate="schoolTextbox" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="passwordTextbox" runat="server" Width="200" Text="Passcode" onfocus="if (this.value == 'Passcode') this.value='';" onblur="if (this.value == '') this.value='Passcode';"/>
                </td>
                <td class='error'>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        InitialValue="Passcode" ErrorMessage="Please enter your passcode" ControlToValidate="passwordTextbox" />
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
