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
        Database db = new Database();
        try
        {

            if (db.authenticate(emailTextbox.Text, passwordTextbox.Text))
            {
                Session["LoggedIn"] = true;
                Server.Transfer("Default.aspx");
            }
            else
            {
                errorMessage = "Incorrect credentials";
            }
        }
        catch (Exception ex)
        {
            errorMessage = String.Format("Internal error: {0}", ex.Message); 
        }

    }

}
