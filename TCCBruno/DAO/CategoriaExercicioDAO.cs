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
    public class CategoriaExercicioDAO
    {
        public List<Categoria_Exercicio> LoadCategoriasExercicio()
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT [categoria_exercicio_id], [nome_categoria_exercicio]" +
                                      " FROM Categoria_Exercicio";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Categoria_Exercicio> categoriasExercicioList = new List<Categoria_Exercicio>();
                    while (reader.Read())
                    {

                        Categoria_Exercicio categoriaExercicio = new Categoria_Exercicio
                        {
                            categoria_exercicio_id = Convert.ToInt32(reader["categoria_exercicio_id"]),
                            nome_categoria_exercicio = reader["nome_categoria_exercicio"].ToString()
                        };
                        categoriasExercicioList.Add(categoriaExercicio);

                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return categoriasExercicioList;
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