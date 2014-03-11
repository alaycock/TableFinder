using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;


public partial class _Default : System.Web.UI.Page
{
    // Variables and structures\
    protected string errorMessage = "";
    protected List<TableGroup> tableList;
    Database db = new Database();

    private string GetAssemblyPath()
    {
        string path;
        path = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        path = path.Substring(6, path.Length - 6);
        return path;
    }

    // Driver method
    protected void Page_Load(object sender, EventArgs e)
    {
        
        tableList = db.getTables();

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

    // Reset entire XML file
    protected void resetChairs(object sender, EventArgs e)
    {
        db.resetChairs();
    }
}
