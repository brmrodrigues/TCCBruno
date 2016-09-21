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
    public class ExercicioDAO
    {

        public List<Exercicio> LoadExerciciosByCategoria(int categoriaId)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT [exercicio_id], [nome_exercicio], [descricao]" +
                                      " FROM Exercicio" +
                                      " WHERE [categoria_exercicio_id] = @pcategoria_exercicio_id";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@pcategoria_exercicio_id", SqlDbType.Int).Value = categoriaId;
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Exercicio> exerciciosList = new List<Exercicio>();
                    while (reader.Read())
                    {

                        Exercicio exercicio = new Exercicio
                        {
                            exercicio_id = Convert.ToInt32(reader["exercicio_id"]),
                            nome_exercicio = reader["nome_exercicio"].ToString(),
                            descricao = reader["descricao"].ToString()
                        };
                        exerciciosList.Add(exercicio);

                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return exerciciosList;
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