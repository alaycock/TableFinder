
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

    public readonly static string EVENT_NAME = "EVENT_NAME";
    public readonly static string EVENT_HOST_NAME = "HOST_NAME";
    public readonly static string EVENT_HOST_EMAIL = "test@test.com";
    public readonly static string EVENT_CHEQUE_PAYABLE = "ORG_NAME";
    public readonly static string URL = "EVENT_URL";

    public readonly static int TOTAL_TABLES = 60;
    public readonly static int SEAT_PRICE = 45;
    public readonly static int SEATS_PER_TABLE = 10;

    public readonly static string SMTP_SERVER = "localhost";
    public readonly static int SMTP_PORT = 25;

    public readonly static string SMTP_FROM_EMAIL = "noreply@tablefinder.info";
    public readonly static string SMTP_FROM_NAME = "tablefinder.info";

    public readonly static string SMTP_CONFIRM_SUBJECT = "Table order confirmation";
    public readonly static string SMTP_CONFIRM_BODY = @"Hello {0} ({1})
The following seats have been saved for you:

{2}

The total cost is: ${3}

If this is not correct, please email any corrections to {4} at {5}
If payment is not received within one week, your order will be canceled.
Please make cheques payable to {6}.";

    public readonly static string SMTP_NEWUSER_SUBJECT = "Please select your tables for {0}";
    public readonly static string SMTP_NEWUSER_BODY = @"Hello,
You've been invited to attend {0} hosted by {1}. Please visit the website {2} and enter your email and access code to select your seats.

Your access code is: {3}

We look forward to hearing from you.

Sincerely,
{4}";

}