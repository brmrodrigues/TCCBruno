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

        public bool InsertTreino(Execucao_Exercicio execucaoExercicio)
        {
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {
                string queryString = "INSERT INTO [dbo].[Execucao_Exercicio] ([exercicio_id], [treino_tipo_id], [series], [repeticoes], [carga], [duracao_descanso])" +
                                        " VALUES (@pexercicio_id, @ptreino_tipo_id, @pseries, @prepeticoes, @pcarga, @pduracao_descanso)";
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.Add("@pexercicio_id", SqlDbType.Int).Value = execucaoExercicio.exercicio_id;
                sqlCommand.Parameters.Add("@ptreino_tipo_id", SqlDbType.Int).Value = execucaoExercicio.treino_tipo_id;
                sqlCommand.Parameters.Add("@pseries", SqlDbType.TinyInt).Value = execucaoExercicio.series;
                sqlCommand.Parameters.Add("@prepeticoes", SqlDbType.SmallInt).Value = execucaoExercicio.repeticoes;
                sqlCommand.Parameters.Add("@pcarga", SqlDbType.TinyInt).Value = execucaoExercicio.carga;
                sqlCommand.Parameters.Add("@pduracao_descanso", SqlDbType.SmallInt).Value = execucaoExercicio.duracao_descanso;

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