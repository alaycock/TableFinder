﻿<%@ page title="Table Selection" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" CodeFile="PrinterFriendly.aspx.cs" Inherits="_Default"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Reservation listing
    </h2>
    <div style="color: Red">
        <%
            String writeValue = "<p>" + errorMessage + "</p>";
            Response.Write(writeValue);
            %>
    </div>

    <div style="margin-top:20px">
    <%
        Response.Write("<table border=1 id='printTable'>" +
            "<thead><tr><th>Table</th><th>Chairs</th><th>Email</th><th>Name</th><th>School</th><th>Phone</th><th>Comment</th></tr></thead></tbody>");
            
        foreach (TableGroup singleTable in tableList.Values)
        {            
            if (singleTable.chairs.Count > 0)
            {
                List<Person> tableOccupants = new List<Person>();
                foreach (Chair singleChair in singleTable.chairs.Distinct())
                {
                    Response.Write("<td>" + singleTable.tableNumber + "</td>");
                    Response.Write("<td>" + singleTable.seatsTaken() + "</td>");
                    Response.Write("<td>" + singleChair.occupant.email + "</td>");
                    Response.Write("<td>" + singleChair.occupant.name + "</td>");
                    Response.Write("<td>" + singleChair.occupant.school + "</td>");
                    Response.Write("<td>" + singleChair.occupant.phone + "</td>");
                    Response.Write("<td>" + singleChair.occupant.comment + "</td></tr>");
                }

            }
        }
        Response.Write("</tbody></table>\n");
         %>
    </div>

    <asp:DataGrid ID="tableListings" runat="server" 
        CellPadding="4" ForeColor="#333333" AutoGenerateColumns="true" Width="900">
    </asp:DataGrid>

</asp:Content>
