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

        public static List<ClassType> SelectQuery<ClassType>(string queryString, List<SqlParameter> parametersList) where ClassType : class, new()
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
                    return GetDataObjects<ClassType>(reader);
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
        }

        public static bool ExecuteNonQuery(string queryString, List<SqlParameter> parametersList)
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

        /// <summary>
        /// Método que retorna Lista de Objetos especificados pelo caller do reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> GetDataObjects<T>(SqlDataReader reader) where T : class, new()
        {
            var list = new List<T>();

            if (reader == null)
                return list;

            HashSet<string> tableColumnNames = null;

            while (reader.Read())
            {
                tableColumnNames = tableColumnNames ?? CollectColumnNames(reader);
                var entity = new T();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    object retrievedObject = null;
                    if (tableColumnNames.Contains(propertyInfo.Name) && (retrievedObject = reader[propertyInfo.Name]) != DBNull.Value)
                    {
                        propertyInfo.SetValue(entity, retrievedObject /*== DBNull.Value ? null : retrievedObject*/, null);
                    }
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// Retirar o nome das colunas resultantes da consulta Select
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static HashSet<string> CollectColumnNames(SqlDataReader reader)
        {
            var collectedColumnInfo = new HashSet<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                collectedColumnInfo.Add(reader.GetName(i));
            }
            return collectedColumnInfo;
        }
    }
}