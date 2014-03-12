<%@ Page Title="Table Selection" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Use the diagram and information below to pick your table
    </h2>
    <div style="color: Red">
        <%
            String writeValue = "<p>" + errorMessage + "</p>";
            Response.Write(writeValue);
            %>
    </div>

    <div style="width:400px; float:left; margin-top:40px;">
    <p>
    Use the drop menus below to select table(s) and number of chairs at each table, click the 'add to cart' button after each table selection, if you need more than one table, repeat the process. Only select submit when you are completed selecting tables and chairs.
    </p>
    <p>There are <% 
                     int available = 0;
                     foreach( TableGroup table in tables )
                         available += table.seatsAvailable();
                         
                     Response.Write(available);
                     %> seats remaining.</p>
    <p>
        Table:
        <asp:DropDownList ID="tableNum" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_changeChairNums" />
        Number of chairs
        <asp:DropDownList ID="chairNum" runat="server" />
        <asp:Button ID="saveSelection" runat="server" OnClick="button_addToCart" Text="Add to cart" />

    </p>
    </div>
    <div style="height:250px; width:300px;float:right;">
    <p>Finished? Submit your order now.</p>
        <table id='personal'>
            <tr><td>Name:</td><td><asp:Label ID="labelName" runat="server" Width="200" /></td></tr>
            <tr><td>Email:</td><td><asp:Label ID="labelEmail" runat="server" Width="200" /></td></tr>
            <tr><td>Phone:</td><td><asp:Label ID="labelPhone" runat="server" Width="200" /></td></tr>
            <tr><td>School:</td><td><asp:Label ID="labelSchool" runat="server" Width="200" /></td></tr>
            <tr><td>Total cost:</td><td>
            <%
                int cost = 0;
                for (int i = 0; i < cartItems.Count; i++)
                {
                    cost += (int)cartItems[i].Second * Config.SEAT_PRICE;
                }
                Session["totalCost"] = cost;
                Response.Write("$" + cost);
                 %>
            </td></tr>
            <tr><td>Comments:</td><td>
                <asp:TextBox ID="comments" runat="server" Rows="3" TextMode="MultiLine" />
            </td></tr>
        </table>
    <asp:Button ID="submitOrder" runat="server" OnClick="button_submitOrder" Text="Submit"/>
    </div>


    <asp:DataGrid ID="ShoppingCart" runat="server" 
        CellPadding="4" ForeColor="#333333"
        OnItemCommand="ShoppingCart_ItemCommand" AutoGenerateColumns="false" Width="500">
        <Columns>
            <asp:BoundColumn DataField="Table Number" HeaderText="Table Number" />
            <asp:BoundColumn DataField="Number of Chairs" HeaderText="Number of Chairs" />
            <asp:ButtonColumn ButtonType="Linkbutton" CommandName="Delete"
                HeaderText="Remove Selection" Text="Click here to remove" HeaderStyle-Width="150" />
        </Columns>
    </asp:DataGrid>

    <div class="circleDisplay" id="topSection" style="position:relative; width:960px; height:700px; margin-top:10px; margin-left:450px;">
    <% // Table images
        int rows = 8;
        int columns = 5;
        for (int rowI = 0; rowI < rows; rowI++)
        {
            Response.Write("<div style=\"width:960px; height:85px; position:relative; float:left;\">\n");
            for (int colI = 0; colI < columns; colI++)
            {
                string tableColorLocation;
                try
                {

                    if (tables[(rowI * columns) + colI].isFull())
                        tableColorLocation = "images/redCircle.png";
                    else
                        tableColorLocation = "images/blueCircle.png";
                }
                catch (Exception e)
                {
                    errorMessage = "Could not create display: " + e.Message;
                    return;
                }
                Response.Write("<div style=\"width:85px; height:85px; position:relative; float:left;text-align: center;\">\n");
                Response.Write("<img src=\"" + tableColorLocation +
                    "\" style=\"left:24px; top:24px; position:absolute;\" width=37 height=37>\n");
                Response.Write("<p style=\"position: relative; margin-left:auto; margin-right:auto; margin-top:32px;\">" + ((rowI * columns) + colI + 1) + "</p>\n");

                for (int chairI = 0; chairI < tables[(rowI * columns) + colI].chairs.Count; chairI++)
                {
                    int leftLoc;
                    int topLoc;

                    double chairAngle = chairI * Math.PI/5;
                    double height = Math.Cos(chairAngle) * 32;
                    double width = Math.Sin(chairAngle) * 32;
                    leftLoc = (int)width - 8 + 42; // -8 for the offset of the size of the chair image, + 42 to correctly place it in around the table
                    topLoc = (int)height - 8 + 42; 
                    
                    Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                        "px; position:absolute; width=16 height=16\" src=\"images/redCircle.png\" width=16 height=16>\n");
                    
                /*
                    else
                    {
                        Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                            "px; position:absolute; width=16 height=16\" src=\"images/blueCircle.png\" width=16 height=16>\n");
                    }
                 */
                }
                Response.Write("</div>");
            }
            Response.Write("</div>");
        }
         %>
    </div>
    
    <img src="images/buffetTables.png" style="position:relative; margin-top:0px; margin-right:50px; float: right; width: 450px; "/>

    <div class="circleDisplay" id="bottomSection" style=" height:350px; float:left; width: 400px;">
    <% // Table images

        int startPosition = 40;
        rows = 4;
        columns = 5;
        for (int rowI = 0; rowI < rows; rowI++)
        {
            Response.Write("<div style=\"width:960px; height:85px; position:relative; float:left;\">\n");
            for (int colI = 0; colI < columns; colI++)
            {
                string tableColorLocation;
                try
                {
                    bool freeSeat = false;
                    for (int i = 0; i < 10; i++) // magic 10 is the number of chairs, I'm a rush, so not doing this right
                    {
                        if (!tables[(rowI * columns) + colI + startPosition].isFull())
                        {
                            freeSeat = true;
                            break;
                        }
                    }

                    if (!freeSeat)
                        tableColorLocation = "images/redCircle.png";
                    else
                        tableColorLocation = "images/blueCircle.png";
                }
                catch (Exception e)
                {
                    errorMessage = "Could not create display: " + e.Message;
                    return;
                }
                Response.Write("<div style=\"width:85px; height:85px; position:relative; float:left;text-align: center;\">\n");
                Response.Write("<img src=\"" + tableColorLocation +
                    "\" style=\"left:24px; top:24px; position:absolute;\" width=37 height=37>\n");
                Response.Write("<p style=\"position: relative; margin-left:auto; margin-right:auto; margin-top:32px;\">" + ((rowI * columns) + colI + startPosition + 1) + "</p>\n");

                System.Diagnostics.Debug.WriteLine(startPosition + " " + tables[(rowI * columns) + colI + startPosition].chairs.Count + "\n");
                for (int chairI = 0; chairI < tables[(rowI * columns) + colI + startPosition].chairs.Count; chairI++)
                {
                    int leftLoc;
                    int topLoc;

                    double chairAngle = chairI * Math.PI/5;
                    double height = Math.Cos(chairAngle) * 32;
                    double width = Math.Sin(chairAngle) * 32;
                    leftLoc = (int)width - 8 + 42; // -8 for the offset of the size of the chair image, + 42 to correctly place it in around the table
                    topLoc = (int)height - 8 + 42; 
                    
                    Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                        "px; position:absolute; width=16 height=16\" src=\"images/redCircle.png\" width=16 height=16>\n");
                        /*
                    else
                    {
                        Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                            "px; position:absolute; width=16 height=16\" src=\"images/blueCircle.png\" width=16 height=16>\n");
                    }
                         * */
                }
                Response.Write("</div>");
            }
            Response.Write("</div>");
        }
         %>
    </div>
</asp:Content>
