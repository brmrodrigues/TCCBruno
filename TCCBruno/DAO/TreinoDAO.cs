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
using TCCBruno.Model;

namespace TCCBruno.DAO
{
    public class TreinoDAO
    {

        public List<Treino> LoadTreinos(int alunoId)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT [treino_id], [data_inicio], [data_fim]" +
                                        " FROM Treino" +
                                        " WHERE [aluno_id] = @paluno_id";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@paluno_id", SqlDbType.Int).Value = alunoId;
                //sqlCommand.Parameters.AddWithValue("@parameter", paramValue);
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Treino> treinosList = new List<Treino>();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                        reader["treino_id"], reader["data_inicio"], reader["data_fim"]);
                        int treinoId = (int)reader["treino_id"];

                        Treino treino = new Treino
                        {
                            treino_id = (int)(reader["treino_id"]),
                            data_inicio = Convert.ToDateTime(reader["data_inicio"]),
                            data_fim = Convert.ToDateTime(reader["data_fim"])
                        };
                        treinosList.Add(treino);

                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return treinosList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool InsertTreino(Treino treino)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "INSERT INTO [dbo].[Treino] ([aluno_id], [data_inicio], [data_fim])" +
                                        " VALUES (@paluno_id, @pdata_inicio, @pdata_fim)";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@paluno_id", SqlDbType.Int).Value = treino.aluno_id;
                sqlCommand.Parameters.Add("@pdata_inicio", SqlDbType.DateTime, 300).Value = treino.data_inicio;
                sqlCommand.Parameters.Add("@pdata_fim", SqlDbType.DateTime).Value = treino.data_fim;

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