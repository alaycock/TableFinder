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
    <b>Remove User</b><br />Enter e-mail of user to remove all instances from the database:<br />
    <asp:TextBox ID="emailToRemove" runat="server" Width="200" />
    PUT A BUTTON IN HERE
    </div>

    <div style="margin-top:20px">
    <b>Add users</b><br />Add a new user to the system<br />
    <asp:TextBox ID="emailToAdd" TextMode="MultiLine" runat="server" Width="200" />
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
    <a href="PrinterFriendly.aspx">Click here to see all reservations (printer friendly).</a>
    </div>

    <div style="margin-top:20px">
    <%
        int counter = 0;
        foreach (TableGroup table in tableList)
        {
            Response.Write("<table>" +
                "<tr><th>Chair</th><th>Email</th><th>Name</th><th>School</th><th>Phone</th><th>Comment</th></tr>");
            foreach(Chair chair in table.chairs)
            {
                Person occupant = chair.occupant;
                Response.Write("<tr><td>" + (++counter) + "</td>");
                Response.Write("<td>" + occupant.email + "</td>");
                Response.Write("<td>" + occupant.name + "</td>");
                Response.Write("<td>" + occupant.school + "</td>");
                Response.Write("<td>" + occupant.phone + "</td>");
                Response.Write("<td>" + occupant.comment + "</td></tr>");
            }
            Response.Write("</table>\n");
        }
         %>
    </div>
</asp:Content>
