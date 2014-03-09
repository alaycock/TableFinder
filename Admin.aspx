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
    <asp:Button ID="button_removeEmail" runat="server" OnClick="removeEmailFromXML" Text="Submit" />
    </div>

    <div style="margin-top:20px">
    <b>Reset Database</b><br />This will reset the ENTIRE DATABASE, a backup will be made:<br />
    <asp:Button ID="resetButton" runat="server" OnClientClick="return confirm('This will remove everything, are you sure?')" OnClick="resetXMLFile" Text="Submit" />
    </div>

    <div style="margin-top:20px">
    <a href="images/layout.PNG" target="_blank">Click here for a the floorplan</a>
    </div>

    <div style="margin-top:20px">
    <a href="PrinterFriendly.aspx">Click here to see all reservations (printer friendly).</a>
    </div>

    <div style="margin-top:20px">
    <%
        for(int i = 0; i < tableList.Count; i++)
        {
            Response.Write("<div onclick=\"openClose('a" + (i + 1) + 
                "')\" class=\"mainExpand\">Table #" + (i + 1) + "</div>\n");
            Response.Write("<div id=\"a" + (i + 1) + "\" class=\"texter\">\n");
            Response.Write("<table>" +
                "<tr><th>Chair</th><th>Email</th><th>Name</th><th>School</th><th>Taken</th><th>Time</th><th>Phone</th><th>Comment</th></tr>");
            for(int j = 0; j < tableList[i].chairs.Count; j++)
            {

                Response.Write("<tr><td>" + (j + 1) + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].email + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].name + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].school + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].taken.ToString() + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].time.ToString() + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].phone + "</td>");
                Response.Write("<td>" + tableList[i].chairs[j].comment + "</td></tr>");
            }
            Response.Write("</table>\n");
            Response.Write("</div>\n");
        }
         %>
    </div>
</asp:Content>
