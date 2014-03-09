﻿using System;
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
    // Variables and structures
    protected const int seatPrice = 45;
    private const string XMLFileLocation = "App_data/XMLFile.xml";

    protected string errorMessage = "";

    protected List<Table> tableList;
    private XMLHandler XMLFile;
    protected List<Pair> cartList = new List<Pair>();
    protected DataView cartView = new DataView();

    // Driver method
    protected void Page_Load(object sender, EventArgs e)
    {
        XMLFile = new XMLHandler(Server.MapPath(XMLFileLocation));

        if (Session.IsNewSession == true || Session["LoggedIn"] == null)
        {
            Session["timedOut"] = true;
            Response.Redirect("login.aspx");
        }

        try
        {
            tableList = XMLFile.Load_XML();
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
            cartList = ViewState["cartlist"] as List<Pair>;

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

        for (int i = 0; i < tableList.Count; i++)
        {
            if (tableList[i].full == false)
            {
                DataRow dr = tablesDataTable.NewRow();
                dr[0] = tableList[i].number;
                dr[1] = tableList[i].number;
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

        ViewState["cartlist"] = cartList;
    }

    // For when the button is clicked to add a table/chair combo to the cart
    protected void button_addToCart(object sender, EventArgs e)
    {
        for (int index = 0; index < cartList.Count; index++)
        {
            if ((int)cartList[index].First == Convert.ToInt32(tableNum.SelectedItem.Text))
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

        cartList.Add(new Pair(Convert.ToInt32(tableNum.SelectedItem.Text),Convert.ToInt32(chairNum.SelectedItem.Text)));

        ViewState["cartlist"] = cartList;

    }

    private DataTable remakeCartDataTable()
    {
        DataTable cartTable = new DataTable();
        cartTable.Columns.Add("Table Number", typeof(string));
        cartTable.Columns.Add("Number of Chairs", typeof(string));

        DataRow row;
        foreach (Pair cartItem in cartList)
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



       

        for (int i = 0; i < tableList[Convert.ToInt32(tableNum.SelectedValue) - 1].chairs.Count; i++)
        {
            if (!tableList[Convert.ToInt32(tableNum.SelectedValue) - 1].chairs[i].taken)
            {
                DataRow dr = chairListDataTable.NewRow();
                dr[0] = freeCounter++ + 1;
                dr[1] = i;
                chairListDataTable.Rows.Add(dr);
            }
        }

        chairNum.DataSource = new DataView(chairListDataTable);
        chairNum.DataTextField = "Text";
        chairNum.DataValueField = "Value";
        chairNum.DataBind();
    }

    private string generateHTMLTableForCart()
    {
        string htmlCart = "<table><tr><th>Table</th><th>Number of Chairs</th></tr>\n";
        foreach (Pair row in cartList)
            if (row.First != null && row.Second != null)
                htmlCart += "<tr><td>" + row.First + "</td><td>" + row.Second + "</td></tr>\n";

        htmlCart += "</table>\n";
        return htmlCart;
    }

    //Currently only used for deleting items, but can be extended to also edit the list of necessary
    protected void ShoppingCart_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        for (int index=0; index < cartList.Count; index++)
        {   
            if ((int)cartList[index].First == Convert.ToInt32(e.Item.Cells[0].Text))
            {
                cartList.RemoveAt(index);
                break;
            }
        }

        remakeCartDataTable();
        cartView = new DataView(remakeCartDataTable());
        ShoppingCart.DataSource = cartView;
        ShoppingCart.DataBind();

        ViewState["cartlist"] = cartList;
    }

    protected void button_submitOrder(object sender, EventArgs e)
    {
        if (cartList.Count == 0)
        {
            errorMessage = "Cart is empty, please select at least one seat.";
            return;
        }

        Session["outputTable"] = generateHTMLTableForCart();

        try
        {
            sendEmail(labelEmail.Text);
            tableList = prepWrite(tableList);
            XMLFile.writeTablelistToXML(tableList);
        }
        catch (Exception ex)
        {
            errorMessage = "Could not store your selection, please try again and ignore the email you have recieved.<br>If the problem persists, please contact Robin Laycock with the following message:<br>" + ex.Message;
            return;
        }

        Session["comment"] = comments.Text;
        Response.Redirect("~/Confirmation.aspx");
    }

    private void sendEmail(string emailAddress)
    {
        int cost = 0;
        for (int i = 0; i < cartList.Count; i++)
        {
            cost += (int)cartList[i].Second * seatPrice;
        }

        MailMessage outgoingMessage = new MailMessage();
        outgoingMessage.From = new MailAddress("noreply@tablefinder.info", "tablefinder"); /////////////// Change this
        outgoingMessage.Bcc.Add("rglaycock@cbe.ab.ca");
        outgoingMessage.To.Add(emailAddress);
        outgoingMessage.Subject = "Table order confirmation";

        string body = "Hello " + labelName.Text + " (" + labelSchool.Text + ")\n";
        body += "The following seats have been saved for you:\n";

        for (int i = 0; i < cartList.Count; i++)
            body += "Table #" + cartList[i].First + " for " + cartList[i].Second + " seats\n";

        body += "\nThe total cost is: $" + cost + "\n\n";
        body += "If this is not correct, please email any corrections to Robin Laycock at rglaycock@cbe.ab.ca\n";
        body += "If payment is not received within one week, your order will be canceled. Make cheques out to John Ware School.\n";

        outgoingMessage.Body = body;

        //NetworkCredential creds = new NetworkCredential("noreply@tablefinder.info", "tqbfjotld1.");

        SmtpClient server = new SmtpClient("relay-hosting.secureserver.net", 25);
        server.EnableSsl = false;
        server.UseDefaultCredentials = true;
        //server.Credentials = creds;
        server.Send(outgoingMessage); //Send email for real
    }

    protected List<Table> prepWrite(List<Table> listToFix)
    {
        foreach (Pair cartElem in cartList)
        {
            //Find the index for the table number
            int tableListIndex = 0;
            for (; tableListIndex < listToFix.Count; tableListIndex++)
            {
                if ((int)cartElem.First == listToFix[tableListIndex].number)
                    break;
            }

            for (int i = 0; i < listToFix[tableListIndex].chairs.Count; i++)
            {
                if (!listToFix[tableListIndex].chairs[i].taken && (int)cartElem.Second > 0)
                {
                    //Because for some stupid reason, I can't edit the chair in the list, I have to remove and make a new one
                    Chair insertChair = new Chair();

                    listToFix[tableListIndex].chairs[i].taken = true;
                    listToFix[tableListIndex].chairs[i].time = DateTime.Now;
                    listToFix[tableListIndex].chairs[i].name = labelName.Text;
                    listToFix[tableListIndex].chairs[i].email = labelEmail.Text;
                    listToFix[tableListIndex].chairs[i].school = labelSchool.Text;
                    listToFix[tableListIndex].chairs[i].phone = labelPhone.Text;
                    listToFix[tableListIndex].chairs[i].comment = comments.Text;

                    cartElem.Second = (int)cartElem.Second - 1;
                }
                if ((int)cartElem.Second == 0)
                    break;
            }

            bool atLeastOneFree = false;
            for (int i = 0; i < listToFix[tableListIndex].chairs.Count; i++)
            {
                if (!listToFix[tableListIndex].chairs[i].taken)
                {
                    atLeastOneFree = true;
                    break;
                }
            }
            if (!atLeastOneFree)
                listToFix[tableListIndex].full = true;
        }
        return listToFix;
    }
}