using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class SqlDatabase
{
    private SqlConnection connection;

    public SqlDatabase()
    {
        connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ShareFun; Integrated Security=SSPI");
    }

    public int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters)
    {
        using (SqlCommand command = CreateCommand(commandText, parameters))
        {
            try
            {
                OpenConnection();
                return command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection();
            }
        }
    }

    public bool ExecuteScalar(string commandText, Dictionary<string, object> parameters)
    {
        using (SqlCommand command = CreateCommand(commandText, parameters))
        {
            try
            {
                OpenConnection();
                return command.ExecuteScalar() != null;
            }
            finally
            {
                CloseConnection();
            }
        }
    }

    public SqlDataReader ExecuteReader(string commandText, Dictionary<string, object> parameters)
    {
        using (SqlCommand command = CreateCommand(commandText, parameters))
        {
            OpenConnection();
            return command.ExecuteReader();
        }
    }

    private void OpenConnection()
    {
        if (connection.State != ConnectionState.Open)
            connection.Open();
    }

    public void CloseConnection()
    {
        if (connection.State == ConnectionState.Open)
            connection.Close();
    }

    private SqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
    {
        SqlCommand command = new SqlCommand(commandText, connection);
        AddParameters(command, parameters);
        return command;
    }

    private void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
    {
        if (parameters == null)
            return;

        foreach (var param in parameters)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = param.Key;
            parameter.Value = param.Value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
