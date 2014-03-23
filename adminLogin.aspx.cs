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
        if( usernameTextbox.Text.Equals(Config.ADMIN_USER) && passwordTextbox.Text.Equals(Config.ADMIN_PASSWORD) )
        {
            Session["AdminLoggedIn"] = true;
            Server.Transfer("Admin.aspx");
        }
        else
        {
            errorMessage = "Incorrect password";
        }

    }

}
