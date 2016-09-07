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

        public Pessoa[] LoadAlunos(int instrutorId)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT A.[aluno_id], P.[nome_pessoa], P.[usuario], P.[status]" +
                                        " FROM Aluno AS A" +
                                        " INNER JOIN Pessoa AS P ON (P.[pessoa_id] = A.[pessoa_id])" +
                                        " WHERE A.[instrutor_id] = " + instrutorId.ToString();
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                //sqlCommand.Parameters.AddWithValue("@parameter", paramValue);
                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Pessoa> pessoasList = new List<Pessoa>();
                    while (reader.Read())
                    {
                        Pessoa pessoa = new Pessoa
                        {
                            aluno_id = (int)reader["aluno_id"],
                            nome_pessoa = reader["nome_pessoa"].ToString(),
                            usuario = reader["usuario"].ToString(),
                            status = (bool)reader["status"]
                        };
                        pessoasList.Add(pessoa);
                        //Console.WriteLine("\t{0}\t{1}\t{2}",
                        //sqlDataReader[0], sqlDataReader[1], sqlDataReader[2]);
                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return pessoasList.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
    }
}