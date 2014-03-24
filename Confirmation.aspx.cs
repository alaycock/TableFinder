using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class About : System.Web.UI.Page
{
    protected string name;
    protected string email;
    protected string school;
    protected int cost;
    protected int seatPrice;
    protected Dictionary<string, string> userInfo;
    protected Dictionary<int, TableGroup> cartItems;

    protected string errorMessage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
        {
            Session["timedOut"] = true;
            Response.Redirect("login.aspx");
        }
        
        userInfo = (Dictionary<string,string>)Session["userInfo"];
        cost = (int)Session["totalCost"];

        cartItems = (Dictionary<int, TableGroup>)Session["cart"];
    }

    protected string generateHTMLTableForCart()
    {
        string htmlCart = "<table><tr><th>Table</th><th>Number of Chairs</th></tr>\n";
        foreach (TableGroup singleTable in cartItems.Values)
            htmlCart += "<tr><td>" + singleTable.tableNumber + "</td><td>" + singleTable.seatsTaken() + "</td></tr>\n";

        htmlCart += "</table>\n";
        return htmlCart;
    }
}
