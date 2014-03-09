using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page
{

    protected string errorMessage = "";

    protected void checkPassword_button(object sender, EventArgs e)
    {
        if (passwordTextbox.Text.Equals("defaultLogin"))
        {
            Session["LoggedIn"] = true;
            Server.Transfer("Default.aspx");
        }
        else if (passwordTextbox.Text.Equals("adminLogin"))
        {
            Session.Clear();
            Session["AdminLoggedIn"] = true;
            Response.Redirect("Admin.aspx");
        }
        else
        {
            errorMessage = "Incorrect password";
        }

    }

}
