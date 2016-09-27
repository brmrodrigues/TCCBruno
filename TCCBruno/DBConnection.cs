using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data.SqlClient;
using System.Data;

namespace TCCBruno
{
    public static class DBConnection
    {
        public static string ConnectionString
        {
            get
            {
                return "Data Source=tcp:tccbruno.database.windows.net,1433;" +
                        "Initial Catalog=Tcc;User ID=tccbruno@tccbruno;Password=dmXrsYc2";
            }
        }

        public static SqlDataReader SelectQuery(string queryString, List<SqlParameter> parametersList)
        {
            SqlConnection connection;
            SqlDataReader reader;
            using (connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                    sqlCommand.Parameters.AddRange(parametersList.ToArray());
                    sqlCommand.CommandType = CommandType.Text;
                    reader = sqlCommand.ExecuteReader();
                }
                catch (InvalidOperationException invalidException)
                {
                    Console.WriteLine(invalidException.Message);
                    return null;
                }
                catch (SqlException sqlException)
                {
                    Console.WriteLine(sqlException.Message);
                    return null;
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                    return null;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }

            return reader;
        }

        public static bool InsertQuery(string queryString, List<SqlParameter> parametersList)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                    sqlCommand.Parameters.AddRange(parametersList.ToArray());
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidOperationException invalidException)
                {
                    Console.WriteLine(invalidException.Message);
                    return false;
                }
                catch (SqlException sqlException)
                {
                    Console.WriteLine(sqlException.Message);
                    return false;
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                    return false;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return false;
                }
            }

            return true;
        }
    }
}