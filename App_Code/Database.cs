using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Text;

/// <summary>
/// Summary description for Database
/// </summary>
public class Database
{
    private MySqlConnection connection;

	public Database()
	{
        string connectionString = String.Format(
            "server={0};user id={1};password={2};persistsecurityinfo=True;database={3};port={4}",
            Config.DBSERVER, Config.DBUSER, Config.DBPASSWORD, Config.DBSCHEMA, Config.DBPORT
            );
        connection = new MySqlConnection(connectionString);
	}

    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            return false;
        }
    }

    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            return false;
        }
    }

    public void resetChairs()
    {
        string query = "DELETE FROM chairs";
        executeNonReadingQuery(query, new List<MySqlParameter>());
    }

    public bool authenticate(string emailInput, string passwordInput)
    {
        string query = "SELECT passwordHash, passwordSalt FROM users WHERE email=@email";
        List<MySqlParameter> parameters = new List<MySqlParameter>();
        parameters.Add(new MySqlParameter("@email", emailInput));

        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);

            foreach (MySqlParameter param in parameters)
                cmd.Parameters.Add(param);

            MySqlDataReader reader = cmd.ExecuteReader();
            byte[] databaseHash = new byte[65535];
            byte[] salt = new byte[32];

            while (reader.Read())
            {
                databaseHash = (byte[])reader["passwordHash"];
                reader.GetBytes(1, 0, salt, 0, 32);
            }

            reader.Close();
            this.CloseConnection();

            return ConfirmPassword(passwordInput, databaseHash, salt);
        }
        return true;
    }

    public void create_user(string emailInput, string passwordInput)
    {
        byte[] salt = new byte[32];
        RandomNumberGenerator RNG = RandomNumberGenerator.Create();
        RNG.GetNonZeroBytes(salt);
        byte[] inputHash = Hash(passwordInput, salt);

        string query = "INSERT INTO users (email, passwordHash, passwordSalt) VALUES (@email, @pwhash, @pwsalt)";
        List<MySqlParameter> parameters = new List<MySqlParameter>();
        parameters.Add(new MySqlParameter("@email", emailInput));
        parameters.Add(new MySqlParameter("@pwhash", inputHash));
        parameters.Add(new MySqlParameter("@pwsalt", salt));

        executeNonReadingQuery(query, parameters);
    }

    public void update_user(string _email, string _name, string _school, string _phone, string _comment)
    {
        string query = "UPDATE users SET name=@name, school=@school, phone=@phone, comment=@comment WHERE email=@email";

        List<MySqlParameter> parameters = new List<MySqlParameter>();
        parameters.Add(new MySqlParameter("@email", _email));
        parameters.Add(new MySqlParameter("@name", _name));
        parameters.Add(new MySqlParameter("@school", _school));
        parameters.Add(new MySqlParameter("@phone", _phone));
        parameters.Add(new MySqlParameter("@comment", _comment));

        executeNonReadingQuery(query, parameters);
    }

    public void purchaseChairs(Dictionary<int, TableGroup> tables, string email)
    {
        

        string query = "INSERT INTO chairs (chairsPurchased, tableNumber, userID) SELECT @chairs, @tableNum, userID FROM users WHERE email=@email";

        foreach (TableGroup table in tables.Values)
        {
            if (getChairsAtTable(table.tableNumber) + table.seatsTaken() > Config.SEATS_PER_TABLE)
                throw new ArgumentException("Your selection is no longer available, please make a new selection.");

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@chairs", table.seatsTaken()));
            parameters.Add(new MySqlParameter("@tableNum", table.tableNumber));
            parameters.Add(new MySqlParameter("@email", email));

            executeNonReadingQuery(query, parameters);
        }
    }

    private int getChairsAtTable(int tableNumber)
    {
        string query = "SELECT SUM(chairsPurchased) as chairsTaken FROM chairs WHERE tableNumber=@tableNumber";
        int chairsTaken = 0;
        List<MySqlParameter> parameters = new List<MySqlParameter>();
        parameters.Add(new MySqlParameter("@tableNumber", tableNumber));

        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);

            foreach (MySqlParameter param in parameters)
                cmd.Parameters.Add(param);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                    chairsTaken = 0;
                else
                    chairsTaken = reader.GetInt32("chairsTaken");
            }

            reader.Close();
            this.CloseConnection();
        }
        else
            throw new Exception("Could not open connection to database.");

        return chairsTaken;
    }

    private void executeNonReadingQuery(string strSQL, List<MySqlParameter> parameters)
    {
        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(strSQL, connection);
            foreach (MySqlParameter param in parameters)
                cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
        else
            throw new Exception("Could not open connection to database.");
    }

    public Dictionary<int, TableGroup> getTables()
    {
        Dictionary<int, TableGroup> tables = new Dictionary<int, TableGroup>();

        string query = "SELECT tableNumber, chairsPurchased, name, email, school, phone, comment FROM chairs LEFT JOIN users ON chairs.userID=users.userID";

        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Person occupant = new Person(
                    getEmptyIfNull(reader, 2),
                    getEmptyIfNull(reader, 3),
                    getEmptyIfNull(reader, 4),
                    getEmptyIfNull(reader, 5),
                    getEmptyIfNull(reader, 6));

                int tableNumber = reader.GetInt32("tableNumber");
                Chair chair = new Chair(occupant);

                if (!tables.Keys.Contains(tableNumber))
                    tables.Add(tableNumber, new TableGroup(tableNumber));

                for (int i = 0; i < reader.GetInt32("chairsPurchased"); i++)
                    tables[tableNumber].addSeat(chair);

            }
            reader.Close();
            this.CloseConnection();
        }


        for (int i = 1; i <= Config.TOTAL_TABLES; i++)
        {
            if (!tables.Keys.Contains(i))
            {
                TableGroup newTable = new TableGroup(i);
                tables.Add(i, newTable);
            }
        }

        return tables;
    }

    public Dictionary<string, string> getUser(string emailInput)
    {
        Dictionary<string, string> returnDict = new Dictionary<string, string>();
        string query = "SELECT email, name, school, phone, comment, lastupdate FROM users WHERE email=@email";
        List<MySqlParameter> parameters = new List<MySqlParameter>();
        parameters.Add(new MySqlParameter("@email", emailInput));

        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);

            foreach (MySqlParameter param in parameters)
                cmd.Parameters.Add(param);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                returnDict.Add("email", getEmptyIfNull(reader, 0));
                returnDict.Add("name", getEmptyIfNull(reader, 1));
                returnDict.Add("school", getEmptyIfNull(reader, 2));
                returnDict.Add("phone", getEmptyIfNull(reader, 3));
                returnDict.Add("comment", getEmptyIfNull(reader, 4));
                returnDict.Add("lastupdate", getEmptyIfNull(reader, 5));
            }

            reader.Close();
            this.CloseConnection();

        }
        return returnDict;
    }

    private string getEmptyIfNull(MySqlDataReader reader, int index)
    {
        if (!reader.IsDBNull(index))
            return reader.GetString(index);
        
        return "";
    }

    private static byte[] Hash(string value, byte[] salt)
    {
        return Hash(Encoding.UTF8.GetBytes(value), salt);
    }

    private static byte[] Hash(byte[] value, byte[] salt)
    {
        byte[] saltedValue = value.Concat(salt).ToArray();
        return new SHA256Managed().ComputeHash(saltedValue);
    }

    private bool ConfirmPassword(string password, byte[] passwordHash, byte[] salt)
    {
        byte[] newPasswordHash = new byte[65535];
        newPasswordHash = Hash(password, salt);
        return passwordHash.SequenceEqual(newPasswordHash);
    }
}