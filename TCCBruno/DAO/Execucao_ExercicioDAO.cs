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
    public class Execucao_ExercicioDAO
    {

        public List<Execucao_Exercicio> LoadExecucaoExercicios(int treinoTipoId)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "SELECT ee.[execucao_exercicio_id], ee.[exercicio_id], ee.[series], ee.[repeticoes], ee.[carga], ee.[duracao_descanso], e.[nome_exercicio]" +
                                      " FROM Execucao_Exercicio as ee INNER JOIN Exercicio as e ON (ee.exercicio_id = e.exercicio_id)" +
                                      " WHERE ee.[treino_tipo_id] = @ptreino_tipo_id";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@ptreino_tipo_id", SqlDbType.Int).Value = treinoTipoId;
                try
                {
                    connection.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Execucao_Exercicio> execucaoExerciciosList = new List<Execucao_Exercicio>();
                    while (reader.Read())
                    {

                        Execucao_Exercicio execucaoExercicio = new Execucao_Exercicio
                        {
                            execucao_exercicio_id = (int)reader["execucao_exercicio_id"],
                            exercicio_id = (int)reader["exercicio_id"],
                            series = (byte)reader["series"],
                            repeticoes = (short)reader["repeticoes"],
                            carga = (short)reader["carga"],
                            duracao_descanso = (short)reader["duracao_descanso"],
                            Exercicio = new Exercicio { nome_exercicio = reader["nome_exercicio"].ToString() }
                        };
                        execucaoExerciciosList.Add(execucaoExercicio);

                    }
                    reader.Close();
                    connection.Close();
                    //ListView aceita apenas Arrays
                    //Pessoa[] pessoasArray = new Pessoa[pessoasList.Count];

                    return execucaoExerciciosList;
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