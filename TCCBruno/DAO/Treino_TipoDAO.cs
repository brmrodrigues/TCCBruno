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
    public class Treino_TipoDAO
    {

        public List<Treino_Tipo> LoadTreino_Tipos(int treinoId)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT [treino_tipo_id], [treino_tipo_nome], [duracao]" +
                                        " FROM Treino_Tipo" +
                                        " WHERE [treino_id] = @ptreino_id";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@ptreino_id", SqlDbType.Int).Value = treinoId;
                //sqlCommand.Parameters.AddWithValue("@parameter", paramValue);
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Treino_Tipo> treinoTiposList = new List<Treino_Tipo>();
                    while (reader.Read())
                    {

                        Treino_Tipo treinoTipo = new Treino_Tipo
                        {
                            treino_tipo_id = (int)reader["treino_tipo_id"],
                            treino_tipo_nome = reader["treino_tipo_nome"].ToString(),
                            duracao = (byte)reader["duracao"]
                        };
                        treinoTiposList.Add(treinoTipo);

                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return treinoTiposList;
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