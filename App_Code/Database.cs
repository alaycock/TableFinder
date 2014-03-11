using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

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
            MySqlDataReader reader = cmd.ExecuteReader();
            byte[] databaseHash = new byte[65535];
            byte[] salt = new byte[32];
            byte[] inputHash;

            reader.Read();
            

            reader.GetBytes(0, 0, databaseHash, 0, 65535);
            reader.GetBytes(1, 0, salt, 0, 32);
            inputHash = GenerateSaltedHash(toBytes(passwordInput), salt);
            

            reader.Close();
            this.CloseConnection();

            return CompareByteArrays(inputHash, databaseHash);
        }
        else
            return false;

    }

    static byte[] toBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public void executeNonReadingQuery(string strSQL, List<MySqlParameter> paramaters)
    {
        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(strSQL, connection);
            foreach (MySqlParameter param in paramaters)
                cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }

    public List<TableGroup> getTables()
    {
        List<TableGroup> tables = new List<TableGroup>();

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

                if (tables.Contains(new TableGroup(tableNumber)))
                {
                    TableGroup existing = tables.Find(x => x.tableNumber.Equals(tableNumber));
                    existing.addSeat(chair);
                }
                else
                {
                    TableGroup newTable = new TableGroup(tableNumber);
                    newTable.addSeat(chair);
                    tables.Add(newTable);
                }

            }
            reader.Close();

            this.CloseConnection();
        }
        return tables;
    }

    private static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
    {
        HashAlgorithm algorithm = new SHA256Managed();

        byte[] plainTextWithSaltBytes =
          new byte[plainText.Length + salt.Length];

        for (int i = 0; i < plainText.Length; i++)
        {
            plainTextWithSaltBytes[i] = plainText[i];
        }
        for (int i = 0; i < salt.Length; i++)
        {
            plainTextWithSaltBytes[plainText.Length + i] = salt[i];
        }

        return algorithm.ComputeHash(plainTextWithSaltBytes);
    }

    private static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
        {
            return false;
        }

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                return false;
            }
        }
        return true;
    }
}