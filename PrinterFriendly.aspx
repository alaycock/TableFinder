<%@ page title="Table Selection" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" CodeFile="PrinterFriendly.aspx.cs" Inherits="_Default"%>

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
            "<thead><tr><th>Table</th><th>Chairs</th><th>Email</th><th>Name</th><th>School</th><th>Taken</th><th>Time</th><th>Phone</th><th>Comment</th></tr></thead></tbody>");
        for(int i = 0; i < tableList.Count; i++)
        {
            Dictionary<string, int> uniqueSeats = new Dictionary<string, int>();
            for(int j = 0; j < tableList[i].chairs.Count; j++)
            {
                try
                {
                    if(tableList[i].chairs[j].email != "")
                        uniqueSeats.Add(tableList[i].chairs[j].email, 1);
                }
                catch (ArgumentException)
                {
                    uniqueSeats[tableList[i].chairs[j].email] += 1;
                }
            }

            if (uniqueSeats.Count > 0)
            {
                Response.Write("<tr><td>" + (i + 1) + "</td>");

                foreach (KeyValuePair<string, int> chairGroup in uniqueSeats)
                {
                    for (int j = 0; j < tableList[i].chairs.Count; j++)
                    {
                        if (chairGroup.Key.Equals(tableList[i].chairs[j].email))
                        {
                            Response.Write("<td>" + chairGroup.Value + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].email + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].name + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].school + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].taken.ToString() + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].time.ToString() + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].phone + "</td>");
                            Response.Write("<td>" + tableList[i].chairs[j].comment + "</td></tr>");
                            break;
                        }

                    }

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
