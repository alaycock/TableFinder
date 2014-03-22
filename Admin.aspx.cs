using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Windows.Forms;
using System.Net.Mail;


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

    protected void addEmail(object sender, EventArgs e)
    {
        try
        {
            string password = Membership.GeneratePassword(12, 3);
            db.create_user(emailToAdd.Text, password);
            sendEmail(emailToAdd.Text, password);
        }
        catch (Exception ex)
        {
            errorMessage = String.Format("An error occurred: {0}", ex.Message);
        }
    }

    private void sendEmail(string emailAddress, string password)
    {
        MailMessage outgoingMessage = new MailMessage();
        outgoingMessage.From = new MailAddress(Config.SMTP_FROM_EMAIL, Config.SMTP_FROM_NAME);
        outgoingMessage.To.Add(emailAddress);
        outgoingMessage.Subject = String.Format(Config.SMTP_NEWUSER_SUBJECT, Config.EVENT_NAME);

        outgoingMessage.Body = String.Format(Config.SMTP_NEWUSER_BODY,
            Config.EVENT_NAME,
            Config.EVENT_HOST_NAME,
            Config.URL,
            password,
            Config.EVENT_HOST_NAME);

        SmtpClient server = new SmtpClient(Config.SMTP_SERVER, Config.SMTP_PORT);
        server.UseDefaultCredentials = true;
        server.EnableSsl = false;
        server.Send(outgoingMessage);
    }

    // Driver method
    protected void Page_Load(object sender, EventArgs e)
    {
        
        tableList = db.getTables();

        if (Session.IsNewSession == true)
        {
            Session["AdminLoggedIn"] = true;
            Response.Redirect("Admin.aspx");
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
