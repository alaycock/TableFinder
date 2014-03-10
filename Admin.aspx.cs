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
    protected List<Table> tableList;
    private XMLHandler XMLFile;

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
        Database db = new Database();
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

    // Remove a particular user (by email address) from the XML file
    protected void removeEmailFromXML(object sender, EventArgs e)
    {
        foreach (Table tb in tableList)
            foreach (Chair ch in tb.chairs)
                if (ch.email.Equals(emailToRemove.Text))
                {
                    ch.school = "";
                    ch.name = "";
                    ch.email = "";
                    ch.taken = false;
                    ch.time = DateTime.MinValue;
                }
        XMLFile.writeTablelistToXML(tableList);
    }

    // Reset entire XML file
    protected void resetXMLFile(object sender, EventArgs e)
    {
        XMLHandler backup = new XMLHandler(Server.MapPath(XMLFileLocation + String.Format(" - BACKUP - {0:yyyy-MM-dd_hh-mm-ss-tt}",DateTime.Now)));
        backup.writeTablelistToXML(tableList);

        foreach (Table tb in tableList)
            foreach (Chair ch in tb.chairs)
            {
                ch.school = "";
                ch.name = "";
                ch.email = "";
                ch.taken = false;
                ch.time = DateTime.MinValue;
                ch.comment = "";
                ch.phone = "";
            }
        XMLFile.writeTablelistToXML(tableList);
        return;
    }
}
