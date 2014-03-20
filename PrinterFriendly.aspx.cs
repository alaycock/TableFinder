using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    private Database db;
    protected string errorMessage = "";
    protected List<TableGroup> tableList;


    // Driver method
    protected void Page_Load(object sender, EventArgs e)
    {
        db = new Database();
        
        if (Session.IsNewSession == true || (bool)Session["AdminLoggedIn"] == false)
        {
            Session["AdminLoggedIn"] = false;
            Response.Redirect("login.aspx");
        }

        try
        {
            tableList = db.getTables();
        }
        catch (Exception ex)
        {
            errorMessage = "Could not load database. <br>" + ex.Message;
        }
    }
}
