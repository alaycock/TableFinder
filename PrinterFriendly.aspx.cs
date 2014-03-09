using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    private XMLHandler XMLFile;
    private const string XMLFileLocation = "App_data/XMLFile.xml";
    protected string errorMessage = "";
    protected List<Table> tableList;


    // Driver method
    protected void Page_Load(object sender, EventArgs e)
    {
        XMLFile = new XMLHandler(Server.MapPath(XMLFileLocation));
        if (Session.IsNewSession == true || (bool)Session["AdminLoggedIn"] == false)
        {
            Session["AdminLoggedIn"] = false;
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
    }
}
