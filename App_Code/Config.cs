using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for config
/// </summary>
public static class Config
{
    public readonly static string DBSERVER = "localhost";
    public readonly static string DBPORT = "3306";
    public readonly static string DBSCHEMA = "tablefinder";
    public readonly static string DBUSER = "dummyuser";
    public readonly static string DBPASSWORD = "dummypassword";

    public readonly static string ADMIN_USER = "dummyuser";
    public readonly static string ADMIN_PASSWORD = "dummypassword";

    public readonly static int TOTAL_TABLES = 60;
    public readonly static int SEAT_PRICE = 45;
    public readonly static int SEATS_PER_TABLE = 10;

    public readonly static string SMTP_SERVER = "relay-hosting.secureserver.net";
    public readonly static int SMTP_PORT = 25;

    public readonly static string SMTP_FROM_EMAIL = "noreply@tablefinder.info";
    public readonly static string SMTP_FROM_NAME = "tablefinder.info";

    public readonly static string SMTP_CONFIRM_NAME = "Dummy Name";
    public readonly static string SMTP_CONFIRM_EMAIL = "dummyemail@example.com";
    public readonly static string SMTP_CONFIRM_SUBJECT = "Table order confirmation";
    public readonly static string SMTP_CONFIRM_BODY = @"Hello {1} ({2})
        The following seats have been saved for you:

        {3}        

        The total cost is: ${4}

        If this is not correct, please email any corrections to {4} at {5}
        If payment is not received within one week, your order will be canceled.";

}