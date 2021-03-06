﻿<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
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
                    <asp:TextBox ID="emailTextbox" runat="server" Width="200" placeholder="Email"/>
                </td>
                <td class='error'>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="emailTextbox" 
                        ValidationExpression="^[a-z0-9][a-z0-9_\.-]{0,}[a-z0-9]@[a-z0-9][a-z0-9_\.-]{0,}[a-z0-9][\.][a-z0-9]{2,4}$" ErrorMessage="Invalid email address"  />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="passwordTextbox" runat="server" Width="200" TextMode="Password" placeholder="Password" />
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
