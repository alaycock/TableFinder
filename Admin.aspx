<%@ Page Title="Table Selection" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Admin.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Admin page
    </h2>
    <div style="color: Red">
        <%
            String writeValue = "<p>" + errorMessage + "</p>";
            Response.Write(writeValue);
            %>
    </div>

    <div style="margin-top:20px">
    <b>Add users</b><br />Add a new user to the system, enter user's email separated by semicolons.<br />
    <asp:TextBox ID="emailToAdd" TextMode="MultiLine" runat="server" Width="500" />
    <asp:Button ID="submit_emailToAdd" runat="server" OnClick="addEmail" Text="Submit" />
    </div>

    <div style="margin-top:20px">
    <b>Reset Database</b><br />This will reset the ENTIRE DATABASE, there is no way of recovering this data<br />
    <asp:Button ID="resetButton" runat="server" OnClientClick="return confirm('This will remove everything, are you sure?')" OnClick="resetChairs" Text="Submit" />
    </div>

    <div style="margin-top:20px">
    <a href="images/layout.PNG" target="_blank">Click here for a the floorplan</a>
    </div>

    <div style="margin-top:20px">
    <a href="PrinterFriendly.aspx" target="_blank">Click here to see all reservations (printer friendly).</a>
    </div>

</asp:Content>
