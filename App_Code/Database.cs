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

    public void executeNonReadingQuery(string strSQL, List<MySqlParameter> paramaters)
    {
        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(strSQL, connection);
            foreach (MySqlParameter param in paramaters)
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

        string query = "SELECT tableNumber, name, email, school, phone, comment FROM chairs LEFT JOIN users ON chairs.userID=users.userID";

        if (this.OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Person occupant = new Person(
                    reader.GetString("name"),
                    reader.GetString("email"),
                    reader.GetString("school"),
                    reader.GetString("phone"),
                    reader.GetString("comment"));
                Chair chair = new Chair(occupant);

                int tableNumber = reader.GetInt32("tableNumber");


                if (tables.Keys.Contains(tableNumber))
                {
                    tables[tableNumber].addSeat(chair);
                }
                else
                {
                    TableGroup newTable = new TableGroup(tableNumber);
                    newTable.addSeat(chair);
                    tables.Add(tableNumber, newTable);
                }
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