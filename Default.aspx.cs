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

    protected List<TableGroup> tables;
    protected List<Pair> cartItems = new List<Pair>();
    protected DataView cartView = new DataView();

    // Driver method
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.IsNewSession == true || Session["LoggedIn"] == null)
        {
            Session["timedOut"] = true;
            Response.Redirect("login.aspx");
        }

        try
        {
            Database db = new Database();
            tables = db.getTables();
        }
        catch (Exception ex)
        {
            errorMessage = "Could not load database. <br>" + ex.Message;
        }

        if (!IsPostBack)
        {
            setup_input_fields();

            if (!string.IsNullOrEmpty(Request.Form[3]))
            {
                Session["name"] = Request.Form[3];
                Session["email"] = Request.Form[4];
                Session["phone"] = Request.Form[5];
                Session["school"] = Request.Form[6];
            }
            else
            {
                errorMessage = "Error retriving user information. Please go back and try again.";
                return;
            }
        }
        else
        {
            errorMessage = ViewState["errormsg"] as String;
            cartItems = ViewState["cartItems"] as List<Pair>;

            cartView = new DataView(remakeCartDataTable());
            ShoppingCart.DataSource = cartView;
            ShoppingCart.DataBind();
        }

        labelName.Text = Session["name"].ToString();
        labelEmail.Text = Session["email"].ToString();
        labelPhone.Text = Session["phone"].ToString();
        labelSchool.Text = Session["school"].ToString();
    }

    // Setup the dropdown lists for tables and chairs
    private void setup_input_fields()
    {
        DataTable tablesDataTable = new DataTable();
        tablesDataTable.Columns.Add(new DataColumn("Text", typeof(String)));
        tablesDataTable.Columns.Add(new DataColumn("Value", typeof(int)));

        for (int i = 0; i < tables.Count; i++)
        {
            if (!tables[i].isFull())
            {
                DataRow dr = tablesDataTable.NewRow();
                dr[0] = tables[i].tableNumber;
                dr[1] = tables[i].tableNumber;
                tablesDataTable.Rows.Add(dr);
            }
        }

        tableNum.DataSource = new DataView(tablesDataTable);
        tableNum.DataTextField = "Text";
        tableNum.DataValueField = "Value";
        tableNum.DataBind();

        load_chair_helper(1);
        
        cartView = new DataView(remakeCartDataTable());
        ShoppingCart.DataSource = cartView;
        ShoppingCart.DataBind();

        ViewState["cartItems"] = cartItems;
    }

    // For when the button is clicked to add a table/chair combo to the cart
    protected void button_addToCart(object sender, EventArgs e)
    {
        for (int index = 0; index < cartItems.Count; index++)
        {
            if ((int)cartItems[index].First == Convert.ToInt32(tableNum.SelectedItem.Text))
            {
                errorMessage = "This table has already been selected. If you wish to add more people, " +
                    "please remove it from your cart, and add it with the total number of people you want " +
                    "at the table.";
                return;
            }
        }

        DataTable cartTable = remakeCartDataTable();

        DataRow row;
        row = cartTable.NewRow();
        row[0] = tableNum.SelectedItem.Text;
        row[1] = chairNum.SelectedItem.Text;
        
        cartTable.Rows.Add(row);

        cartView = new DataView(cartTable);

        ShoppingCart.DataSource = cartView;
        ShoppingCart.DataBind();

        cartItems.Add(new Pair(Convert.ToInt32(tableNum.SelectedItem.Text),Convert.ToInt32(chairNum.SelectedItem.Text)));

        ViewState["cartItems"] = cartItems;

    }

    private DataTable remakeCartDataTable()
    {
        DataTable cartTable = new DataTable();
        cartTable.Columns.Add("Table Number", typeof(string));
        cartTable.Columns.Add("Number of Chairs", typeof(string));

        DataRow row;
        foreach (Pair cartItem in cartItems)
        {
            row = cartTable.NewRow();
            row[0] = cartItem.First;
            row[1] = cartItem.Second;
            cartTable.Rows.Add(row);
        }
        return cartTable;
    }

    // When the first dropdown list is changed, update so the second one has the correct number of seats shown
    protected void ddl_changeChairNums(object sender, EventArgs e)
    {
        int selectedTable = Convert.ToInt32(tableNum.SelectedValue.ToString());
        load_chair_helper(selectedTable);
    }

    // Show the correct number of seats available per table
    private void load_chair_helper(int selectedTable)
    {
        DataTable chairListDataTable = new DataTable();
        chairListDataTable.Columns.Add(new DataColumn("Text", typeof(String)));
        chairListDataTable.Columns.Add(new DataColumn("Value", typeof(int)));

        int freeCounter = 0;


        string result = "";
        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(tableNum))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(tableNum);
            result += name + " = " + value + ", ";
            
        }

        for (int i = 0; i < tables[Convert.ToInt32(tableNum.SelectedValue) - 1].chairs.Count; i++)
        {
            DataRow dr = chairListDataTable.NewRow();
            dr[0] = freeCounter++ + 1;
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
        foreach (Pair row in cartItems)
            if (row.First != null && row.Second != null)
                htmlCart += "<tr><td>" + row.First + "</td><td>" + row.Second + "</td></tr>\n";

        htmlCart += "</table>\n";
        return htmlCart;
    }

    //Currently only used for deleting items, but can be extended to also edit the list of necessary
    protected void ShoppingCart_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        for (int index=0; index < cartItems.Count; index++)
        {   
            if ((int)cartItems[index].First == Convert.ToInt32(e.Item.Cells[0].Text))
            {
                cartItems.RemoveAt(index);
                break;
            }
        }

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
            sendEmail(labelEmail.Text);

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
            cost += (int)cartItems[i].Second * Config.SEAT_PRICE;
        return cost;
    }

    private string generateSeatString()
    {
        string seatString = "";
        for (int i = 0; i < cartItems.Count; i++)
            seatString += "Table #" + cartItems[i].First + " for " + cartItems[i].Second + " seats\n";
        return seatString;
    }

    private void sendEmail(string emailAddress)
    {
        int cost = calculateCost();

        MailMessage outgoingMessage = new MailMessage();
        outgoingMessage.From = new MailAddress(Config.SMTP_FROM_EMAIL, Config.SMTP_FROM_NAME);
        outgoingMessage.Bcc.Add(Config.SMTP_CONFIRM_EMAIL);
        outgoingMessage.To.Add(emailAddress);
        outgoingMessage.Subject = Config.SMTP_CONFIRM_SUBJECT;

        string seatString = generateSeatString();

        outgoingMessage.Body = String.Format(Config.SMTP_CONFIRM_BODY,
            labelName.Text,
            labelSchool.Text,
            seatString,
            cost,
            Config.SMTP_CONFIRM_NAME,
            Config.SMTP_CONFIRM_EMAIL);

        SmtpClient server = new SmtpClient(Config.SMTP_SERVER, Config.SMTP_PORT);
        server.EnableSsl = false;
        server.UseDefaultCredentials = true;
        server.Send(outgoingMessage);
    }
}
