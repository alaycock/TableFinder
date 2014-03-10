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

}