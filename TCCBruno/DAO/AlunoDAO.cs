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
using TCCBruno.Model;
using System.Data.SqlClient;
using System.Data;

namespace TCCBruno.DAO
{
    public class AlunoDAO
    {

        public bool InsertAluno(int instrutorId, Pessoa newPessoa)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "INSERT INTO [dbo].[Pessoa] ([nome_pessoa], [usuario], [senha], [status])" +
                                        " OUTPUT Inserted.pessoa_id VALUES (@pnome_pessoa, @pusuario, @psenha, @pstatus)";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@pnome_pessoa", SqlDbType.NVarChar, 300).Value = newPessoa.nome_pessoa;
                sqlCommand.Parameters.Add("@pusuario", SqlDbType.NVarChar, 300).Value = newPessoa.usuario;
                sqlCommand.Parameters.Add("@psenha", SqlDbType.NVarChar).Value = newPessoa.senha;
                sqlCommand.Parameters.Add("@pstatus", SqlDbType.Bit).Value = newPessoa.status;

                //sqlCommand.Parameters.AddWithValue("@parameter", paramValue);
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    int alunoId = (int)sqlCommand.ExecuteScalar();
                    connection.Close();
                    Aluno newAluno = new Aluno
                    {
                        pessoa_id = alunoId,
                        instrutor_id = instrutorId
                    };
                    if (!InserAlunoFromPessoa(newAluno))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }

        private bool InserAlunoFromPessoa(Aluno newAluno)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "INSERT INTO [dbo].[Aluno] ([pessoa_id], [instrutor_id])" +
                                        " VALUES (@ppessoa_id, @pinstrutor_id)";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@ppessoa_id", SqlDbType.Int).Value = newAluno.pessoa_id;
                sqlCommand.Parameters.Add("@pinstrutor_id", SqlDbType.Int).Value = newAluno.instrutor_id;


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
                    return false;
                }
            }
            return true;
        }
    }
}