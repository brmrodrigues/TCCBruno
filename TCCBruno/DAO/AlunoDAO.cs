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

        public bool InsertPessoa(int instrutorId, Pessoa newPessoa, string dataNascimento)
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

                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    int alunoId = (int)sqlCommand.ExecuteScalar();
                    connection.Close();
                    Aluno newAluno = new Aluno
                    {
                        pessoa_id = alunoId,
                        instrutor_id = instrutorId,
                        data_nascimento = dataNascimento
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
            //SqlConnection connection;
            //using (connection = new SqlConnection(DBConnection.ConnectionString))
            //{
            string queryString = "INSERT INTO [dbo].[Aluno] ([pessoa_id], [instrutor_id], [data_nascimento])" +
                                    " VALUES (@ppessoa_id, @pinstrutor_id, @pdata_nascimento)";

            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@ppessoa_id", SqlDbType = SqlDbType.Int, Value = newAluno.pessoa_id },
                new SqlParameter() {ParameterName="@pinstrutor_id", SqlDbType = SqlDbType.Int, Value = newAluno.instrutor_id },
                new SqlParameter() {ParameterName="@pdata_nascimento", SqlDbType = SqlDbType.DateTime, Value = newAluno.data_nascimento }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
            //    SqlCommand sqlCommand = new SqlCommand(queryString, connection);
            //    sqlCommand.Parameters.Add("@ppessoa_id", SqlDbType.Int).Value = newAluno.pessoa_id;
            //    sqlCommand.Parameters.Add("@pinstrutor_id", SqlDbType.Int).Value = newAluno.instrutor_id;


            //    try
            //    {
            //        connection.Open();
            //        sqlCommand.CommandType = CommandType.Text;
            //        sqlCommand.ExecuteNonQuery();
            //        connection.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        return false;
            //    }
            //}
            //return true;
        }

        public List<Aluno> LoadAlunos(int instrutorId)
        {
            SqlConnection connection;
            SqlDataReader reader;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT A.[aluno_id], A.[instrutor_id], A.[data_nascimento], P.[pessoa_id], P.[nome_pessoa], P.[usuario], P.[status]" +
                                        " FROM Aluno AS A" +
                                        " INNER JOIN Pessoa AS P ON (P.[pessoa_id] = A.[pessoa_id])" +
                                        " WHERE A.[instrutor_id] = @pinstrutor_id";
                List<SqlParameter> parametersList = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName="@pinstrutor_id", SqlDbType = SqlDbType.Int, Value = instrutorId }
                };
                //sqlCommand.Parameters.AddWithValue("@pinstrutor_id", instrutorId);
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                    sqlCommand.Parameters.AddRange(parametersList.ToArray());
                    sqlCommand.CommandType = CommandType.Text;
                    reader = sqlCommand.ExecuteReader();
                    List<Aluno> pessoasList = new List<Aluno>();
                    while (reader.Read())
                    {
                        Aluno aluno = new Aluno
                        {
                            aluno_id = (int)reader["aluno_id"],
                            instrutor_id = (int)reader["instrutor_id"],
                            data_nascimento = reader["data_nascimento"].ToString(),
                            Pessoa = new Pessoa
                            {
                                pessoa_id = (int)reader["pessoa_id"],
                                nome_pessoa = reader["nome_pessoa"].ToString(),
                                usuario = reader["usuario"].ToString(),
                                status = (bool)reader["status"]
                            }
                        };
                        pessoasList.Add(aluno);
                        //Console.WriteLine("\t{0}\t{1}\t{2}",
                        //sqlDataReader[0], sqlDataReader[1], sqlDataReader[2]);
                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return pessoasList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public string GetNomeAluno(int alunoId)
        {

            string queryString = "SELECT P.nome_pessoa" +
                                 " FROM Pessoa as P" +
                                 " INNER JOIN Aluno as A on (P.pessoa_id = A.pessoa_id)" +
                                 " WHERE A.aluno_id = @paluno_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@paluno_id", SqlDbType = SqlDbType.Int, Value = alunoId }
            };

            var aluno = DBConnection.SelectQuery<Pessoa>(queryString, parametersList);

            //Return First Or Default
            return aluno[0].nome_pessoa;
        }

        public bool ActivateDeactivateAluno(int pessoaId, bool alunoStatus)
        {
            string queryString = "UPDATE Pessoa" +
                                    " SET status = @pnewStatus" +
                                    " WHERE [pessoa_id] = @ppessoa_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@pnewStatus", SqlDbType = SqlDbType.Bit, Value = !alunoStatus },
                new SqlParameter() {ParameterName="@ppessoa_id", SqlDbType = SqlDbType.Int, Value = pessoaId }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
        }

        public bool RemoveAluno(int pessoaId)
        {
            string queryString = "DELETE FROM Pessoa" +
                                          " WHERE [pessoa_id] = @ppessoa_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@ppessoa_id", SqlDbType = SqlDbType.Int, Value = pessoaId }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
        }

    }
}