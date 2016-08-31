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
using TCCBruno.DAO.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace TCCBruno.DAO
{
    public class Aluno : IAluno
    {
        public string nome_aluno { get; set; }
        public bool status { get; set; }
        public int id { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }

        public bool InsertAluno(Aluno newAluno)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "INSERT INTO [dbo].[ALUNO] ([nome_aluno], [status], [usuario], [senha])" +
                                        " VALUES (@pnome_aluno, @pstatus, @pusuario, @psenha)";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@pnome_aluno", SqlDbType.NVarChar, 100).Value = newAluno.nome_aluno;
                sqlCommand.Parameters.Add("@pstatus", SqlDbType.Bit).Value = newAluno.status;
                sqlCommand.Parameters.Add("@pusuario", SqlDbType.NVarChar, 100).Value = newAluno.usuario;
                sqlCommand.Parameters.Add("@psenha", SqlDbType.NVarChar, 100).Value = newAluno.senha;
                //sqlCommand.Parameters.AddWithValue("@parameter", paramValue);
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return true;
        }
    }

}