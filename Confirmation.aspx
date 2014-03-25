<%@ Page Title="Thank you for your order" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Confirmation.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <style type="text/css">
    table.information {
	    border-width: 0px;
	    border-spacing: 0px;
	    border-style: outset;
	    border-color: white;
	    border-collapse: collapse;
	    background-color: white;
    }
    table.information th {
	    border-width: 0px;
	    padding: 1px;
	    border-style: inset;
	    border-color: gray;
	    background-color: white;
    }
    table.information td {
	    border-width: 0px;
	    padding: 1px;
	    border-style: inset;
	    border-color: gray;
	    background-color: white;
    }
    </style>

    <h2>
        This confirms your order for <% Response.Write(Config.EVENT_NAME); %>
    </h2>
    <h3>
        <a href="login.aspx">Logout</a>
    </h3>

    <p>Please make cheques out to <% Response.Write(Config.EVENT_CHEQUE_PAYABLE); %>, and they should be received within one week of your order.</p>
    <table class="information">
    <tr><td><b>Name:</b></td><td><% Response.Write(userInfo["name"].ToString()); %></td></tr>
    <tr><td><b>Email:</b></td><td><% Response.Write(userInfo["email"].ToString()); %></td></tr>
    <tr><td><b>Phone:</b></td><td><% Response.Write(userInfo["phone"].ToString()); %></td></tr>
    <tr><td><b>School:</b></td><td><% Response.Write(userInfo["school"].ToString()); %></td></tr>
    <tr><td><b>Comments:</b></td><td><% Response.Write(userInfo["comment"].ToString()); %></td></tr>
    <tr><td><b>Payment due:</b></td><td>$<% Response.Write(Session["totalCost"].ToString()); %></td></tr>
    </table>

    <br />

    <% Response.Write(generateHTMLTableForCart()); %>

    <p>If any of the above information is incorrect, please email <% Response.Write(Config.EVENT_HOST_NAME); %> at <% Response.Write(Config.EVENT_HOST_EMAIL); %>.</p>
    <p>Please do not use the back button as it may cause you to resubmit your seat selection.</p>
</asp:Content>
