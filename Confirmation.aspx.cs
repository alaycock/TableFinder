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
    protected string costString;
    protected int seatPrice;

    protected string errorMessage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
        {
            Session["timedOut"] = true;
            Response.Redirect("login.aspx");
        }
    }
}
