using System;
using System.Data.SqlClient;
using System.Diagnostics;

public static class Database
{
    private static string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=ShareFun; Integrated Security=SSPI";

    public static bool AuthenticateUser(string login, string passwordHash)
    {
        var connection = Connect();
        if (connection == null)
            return false;
        string commandText = "SELECT TOP 1 * FROM Users WHERE Login = '" + login + "' AND Password = '" + passwordHash + "'";
        SqlCommand command = new SqlCommand(commandText, connection);
        return command.ExecuteScalar() != null;
    }

    private static SqlConnection Connect()
    {
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}
