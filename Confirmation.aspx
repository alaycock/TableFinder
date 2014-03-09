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
        This confirms your order for the athletic banquet
    </h2>
    <p>Please make cheques out to John Ware School, and they should be received within one week of your order. You will also receive an email with this information.</p>
    <table class="information">
    <tr><td><b>Name:</b></td><td><% Response.Write(Session["name"].ToString()); %></td></tr>
    <tr><td><b>Email:</b></td><td><% Response.Write(Session["email"].ToString()); %></td></tr>
    <tr><td><b>Phone:</b></td><td><% Response.Write(Session["phone"].ToString()); %></td></tr>
    <tr><td><b>School:</b></td><td><% Response.Write(Session["school"].ToString()); %></td></tr>
    <tr><td><b>Comments:</b></td><td><% Response.Write(Session["comment"].ToString()); %></td></tr>
    <tr><td><b>Payment due:</b></td><td>$<% Response.Write(Session["totalCost"].ToString()); %></td></tr>
    </table>

    <br />

    <% Response.Write(Session["outputTable"]); %>

    <p>If any of the above information is incorrect, please email Robin Laycock at <a href="mailto:rglaycock@cbe.ab.ca">rglaycock@cbe.ab.ca</a>.</p>
</asp:Content>
