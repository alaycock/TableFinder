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
                     foreach( TableGroup table in tables.Values )
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
            <tr><td>Email:</td><td><asp:TextBox ID="emailTextbox" Enabled="false" runat="server" Width="200" /></td></tr>
            <tr><td>Name:</td><td><asp:TextBox ID="nameTextbox" runat="server" Width="200" /></td></tr>
            <tr><td>Phone:</td><td><asp:TextBox ID="phoneTextbox" runat="server" Width="200" /></td></tr>
            <tr><td>School:</td><td><asp:TextBox ID="schoolTextbox" runat="server" Width="200" /></td></tr>
            <tr><td>Total cost:</td><td>
            <%
                int cost = 0;
                foreach(TableGroup singleTable in cartItems.Values)
                    cost += singleTable.seatsTaken() * Config.SEAT_PRICE;

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
        OnItemCommand="ShoppingCart_ItemCommand" AutoGenerateColumns="false" Width="00">
        <Columns>
            <asp:BoundColumn DataField="Table Number" HeaderText="Table Number" />
            <asp:BoundColumn DataField="Number of Chairs" HeaderText="Number of Chairs" />
            <asp:ButtonColumn ButtonType="Linkbutton" CommandName="Delete"
                HeaderText="Remove Selection" Text="Click here to remove" HeaderStyle-Width="150" />
        </Columns>
    </asp:DataGrid>

    <div class="circleDisplay" id="topSection" style="position:relative; width:960px; height:700px; margin-top:10px; margin-left:450px;">
    <% renderTables(8, 5, 1); %>
    </div>
    
    <img src="images/buffetTables.png" style="position:relative; margin-top:0px; margin-right:50px; float: right; width: 450px; "/>

    <div class="circleDisplay" id="bottomSection" style=" height:350px; float:left; width: 400px;">
    <% renderTables(4, 5, 41); %>
    </div>
</asp:Content>
