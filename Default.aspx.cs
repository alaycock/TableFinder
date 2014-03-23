using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;


public partial class _Default : System.Web.UI.Page
{

    protected string errorMessage = "";

    protected Dictionary<string, string> userInfo;
    protected Dictionary<int, TableGroup> tables;
    protected Dictionary<int, TableGroup> cartItems = new Dictionary<int, TableGroup>();
    protected DataView cartView = new DataView();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.IsNewSession == true || Session["LoggedIn"] == null)
            Response.Redirect("login.aspx");

        //try
        //{
            Database db = new Database();
            tables = db.getTables();
            userInfo = db.getUser(Session["email"].ToString());
        /*}
        catch (Exception ex)
        {
            errorMessage = "Could not load database: " + ex.Message;
        }*/
        
        if (!IsPostBack)
        {
            reloadSelectionDropdown();

            emailTextbox.Text = userInfo["email"];
            nameTextbox.Text = userInfo["name"];
            phoneTextbox.Text = userInfo["phone"];
            schoolTextbox.Text = userInfo["school"];
            comments.Text = userInfo["comment"];
        }
        else
        {
            errorMessage = ViewState["errormsg"] as String;
            cartItems = ViewState["cartItems"] as Dictionary<int, TableGroup>;

            cartView = new DataView(remakeCartDataTable());
            ShoppingCart.DataSource = cartView;
            ShoppingCart.DataBind();
        }
    }

    private bool isNull(Object obj)
    {
        return obj == null;
    }

    // Setup the dropdown lists for tables and chairs
    private void reloadSelectionDropdown()
    {
        DataTable tablesDataTable = new DataTable();
        tablesDataTable.Columns.Add(new DataColumn("Text", typeof(String)));
        tablesDataTable.Columns.Add(new DataColumn("Value", typeof(int)));

        foreach (TableGroup singleTable in tables.Values)
        {
            int seatsAvailable = singleTable.seatsAvailable();

            if (cartItems.Keys.Contains(singleTable.tableNumber))
                seatsAvailable -= cartItems[singleTable.tableNumber].seatsTaken();

            if (seatsAvailable > 0)
            {
                DataRow dr = tablesDataTable.NewRow();
                dr[0] = singleTable.tableNumber;
                dr[1] = singleTable.tableNumber;
                tablesDataTable.Rows.Add(dr);
            }
        }

        tableNum.DataSource = new DataView(tablesDataTable);
        tableNum.DataTextField = "Text";
        tableNum.DataValueField = "Value";
        tableNum.DataBind();

        reloadChairOptions( Convert.ToInt32(tableNum.SelectedValue.ToString()) );
        
        cartView = new DataView(remakeCartDataTable());
        ShoppingCart.DataSource = cartView;
        ShoppingCart.DataBind();

        ViewState["cartItems"] = cartItems;
    }



    // For when the button is clicked to add a table/chair combo to the cart
    protected void button_addToCart(object sender, EventArgs e)
    {
        int chairsSelected = Convert.ToInt32(chairNum.SelectedItem.Text);
        int tableNumber = Convert.ToInt32(tableNum.SelectedItem.Text);

        int seatsAvailable = cartItems.Keys.Contains(tableNumber) ?
            cartItems[tableNumber].seatsAvailable() : Config.SEATS_PER_TABLE;

        if (tableNumber > Config.TOTAL_TABLES || chairsSelected > seatsAvailable)
            throw new IndexOutOfRangeException("Number of tables or chairs exceeds the limit.");

        TableGroup newTable;
        if (cartItems.Keys.Contains(tableNumber))
        {
            chairsSelected += cartItems[tableNumber].seatsTaken();
            cartItems.Remove(tableNumber);
        }

        newTable = new TableGroup(tableNumber);

        for (int i = 0; i < chairsSelected; i++)
        {
            Chair newSeat = new Chair(null);
            newTable.addSeat(newSeat);
        }

        cartItems.Add(tableNumber, newTable);

        reloadSelectionDropdown();

        DataTable cartTable = remakeCartDataTable();

        cartView = new DataView(cartTable);

        ShoppingCart.DataSource = cartView;
        ShoppingCart.DataBind();

        ViewState["cartItems"] = cartItems;

    }

    private DataTable remakeCartDataTable()
    {
        DataTable cartTable = new DataTable();
        cartTable.Columns.Add("Table Number", typeof(string));
        cartTable.Columns.Add("Number of Chairs", typeof(string));

        DataRow row;
        foreach (TableGroup cartItem in cartItems.Values)
        {
            row = cartTable.NewRow();
            row[0] = cartItem.tableNumber;
            row[1] = cartItem.seatsTaken();
            cartTable.Rows.Add(row);
        }
        return cartTable;
    }

    // When the first dropdown list is changed, update so the second one has the correct number of seats shown
    protected void ddl_changeChairNums(object sender, EventArgs e)
    {
        int selectedTable = Convert.ToInt32(tableNum.SelectedValue.ToString());
        reloadChairOptions(selectedTable);
    }

    // Show the correct number of seats available per table
    private void reloadChairOptions(int selectedTable)
    {
        DataTable chairListDataTable = new DataTable();
        chairListDataTable.Columns.Add(new DataColumn("Text", typeof(String)));
        chairListDataTable.Columns.Add(new DataColumn("Value", typeof(int)));

        int freeCounter = 0;
        int seatsAvailable = tables[selectedTable].seatsAvailable();
        if (cartItems.Keys.Contains(selectedTable))
            seatsAvailable -= cartItems[selectedTable].seatsTaken();

        for (int i = 0; i < seatsAvailable; i++)
        {
            DataRow dr = chairListDataTable.NewRow();
            dr[0] = 1 + freeCounter++;
            dr[1] = i;
            chairListDataTable.Rows.Add(dr);
        }

        chairNum.DataSource = new DataView(chairListDataTable);
        chairNum.DataTextField = "Text";
        chairNum.DataValueField = "Value";
        chairNum.DataBind();
    }

    private string generateHTMLTableForCart()
    {
        string htmlCart = "<table><tr><th>Table</th><th>Number of Chairs</th></tr>\n";
        foreach (TableGroup singleTable in cartItems.Values)
            htmlCart += "<tr><td>" + singleTable.tableNumber + "</td><td>" + singleTable.seatsTaken() + "</td></tr>\n";

        htmlCart += "</table>\n";
        return htmlCart;
    }

    //Currently only used for deleting items, but can be extended to also edit the list of necessary
    protected void ShoppingCart_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        cartItems.Remove(Convert.ToInt32(e.Item.Cells[0].Text));

        reloadSelectionDropdown();
        remakeCartDataTable();
        cartView = new DataView(remakeCartDataTable());
        ShoppingCart.DataSource = cartView;
        ShoppingCart.DataBind();

        ViewState["cartItems"] = cartItems;
    }

    protected void button_submitOrder(object sender, EventArgs e)
    {
        if (cartItems.Count == 0)
        {
            errorMessage = "Cart is empty, please select at least one seat.";
            return;
        }

        Session["outputTable"] = generateHTMLTableForCart();

        try
        {
            //sendEmail(labelEmail.Text);

            // Redo this stuff to actually submit to the DB.

            //tables = prepWrite(tables);
            //XMLFile.writetablesToXML(tables);
        }
        catch (Exception ex)
        {
            errorMessage = "Could not store your selection, please try again and ignore the email you have recieved.<br>If the problem persists, please contact Robin Laycock with the following message:<br>" + ex.Message;
            return;
        }

        Session["comment"] = comments.Text;
        Response.Redirect("~/Confirmation.aspx");
    }

    private int calculateCost()
    {
        int cost = 0;
        for (int i = 0; i < cartItems.Count; i++)
            cost += (int)cartItems[i].seatsTaken() * Config.SEAT_PRICE;
        return cost;
    }

    private string generateSeatString()
    {
        string seatString = "";
        foreach (TableGroup singleTable in cartItems.Values)
            seatString += "Table #" + singleTable.tableNumber + " for " + singleTable.seatsTaken() + " seats\n";
        return seatString;
    }

    private void sendEmail(string emailAddress)
    {
        int cost = calculateCost();

        MailMessage outgoingMessage = new MailMessage();
        outgoingMessage.From = new MailAddress(Config.SMTP_FROM_EMAIL, Config.SMTP_FROM_NAME);
        outgoingMessage.Bcc.Add(Config.EVENT_HOST_EMAIL);
        outgoingMessage.To.Add(emailAddress);
        outgoingMessage.Subject = Config.SMTP_CONFIRM_SUBJECT;

        string seatString = generateSeatString();

        outgoingMessage.Body = String.Format(Config.SMTP_CONFIRM_BODY,
            nameTextbox.Text, 
            schoolTextbox.Text,
            seatString,
            cost,
            Config.EVENT_HOST_NAME,
            Config.EVENT_HOST_EMAIL);

        SmtpClient server = new SmtpClient(Config.SMTP_SERVER, Config.SMTP_PORT);
        server.EnableSsl = false;
        server.UseDefaultCredentials = true;
        server.Send(outgoingMessage);
    }

    public void renderTables(int rows, int columns, int startingIndex)
    {
        for (int rowI = 0; rowI < rows; rowI++)
        {
            Response.Write("<div style=\"width:960px; height:85px; position:relative; float:left;\">\n");
            for (int colI = 0; colI < columns; colI++)
            {
                string tableColorLocation;
                int tableIndex = (rowI * columns) + colI + startingIndex;

                if (tables[tableIndex].isFull())
                    tableColorLocation = "images/redCircle.png";
                else if (cartItems.Keys.Contains(tableIndex) && Config.SEATS_PER_TABLE == tables[tableIndex].seatsTaken() + cartItems[tableIndex].seatsTaken())
                    tableColorLocation = "images/yellowCircle.png";
                else
                    tableColorLocation = "images/blueCircle.png";

                Response.Write("<div style=\"width:85px; height:85px; position:relative; float:left;text-align: center;\">\n");
                Response.Write("<img src=\"" + tableColorLocation +
                    "\" style=\"left:24px; top:24px; position:absolute;\" width=37 height=37>\n");
                Response.Write("<p style=\"position: relative; margin-left:auto; margin-right:auto; margin-top:32px;\">" + (tableIndex) + "</p>\n");

                for (int chairI = 0; chairI < Config.SEATS_PER_TABLE; chairI++)
                {
                    double chairAngle = chairI * Math.PI / 5;
                    double height = Math.Cos(chairAngle) * 32;
                    double width = Math.Sin(chairAngle) * 32;
                    int leftLoc = (int)width - 8 + 42; // -8 for the offset of the size of the chair image, + 42 to correctly place it in around the table
                    int topLoc = (int)height - 8 + 42;

                    if (chairI > tables[tableIndex].seatsAvailable())
                    {
                        Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                            "px; position:absolute; width=16 height=16\" src=\"images/redCircle.png\" width=16 height=16>\n");
                    }
                    else if (cartItems.Keys.Contains(tableIndex) && chairI < tables[tableIndex].seatsTaken() + cartItems[tableIndex].seatsTaken())
                    {
                        Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                         "px; position:absolute; width=16 height=16\" src=\"images/yellowCircle.png\" width=16 height=16>\n");
                    }
                    else
                    {
                        Response.Write("<img style=\"left:" + leftLoc + "px; top:" + topLoc +
                            "px; position:absolute; width=16 height=16\" src=\"images/blueCircle.png\" width=16 height=16>\n");
                    }
                }
                Response.Write("</div>");
            }
            Response.Write("</div>");
        }
    }
}




