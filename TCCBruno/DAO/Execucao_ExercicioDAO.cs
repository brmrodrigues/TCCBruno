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

        public bool InsertExecucaoExercicio(Execucao_Exercicio execucaoExercicio)
        {
            string queryString = "INSERT INTO [dbo].[Execucao_Exercicio] ([exercicio_id], [treino_tipo_id], [series], [repeticoes], [carga], [duracao_descanso])" +
                                    " VALUES (@pexercicio_id, @ptreino_tipo_id, @pseries, @prepeticoes, @pcarga, @pduracao_descanso)";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@pexercicio_id", SqlDbType = SqlDbType.Int, Value = execucaoExercicio.exercicio_id },
                new SqlParameter() {ParameterName="@ptreino_tipo_id", SqlDbType = SqlDbType.Int, Value = execucaoExercicio.treino_tipo_id },
                new SqlParameter() {ParameterName="@pseries", SqlDbType = SqlDbType.TinyInt, Value = execucaoExercicio.series },
                new SqlParameter() {ParameterName="@prepeticoes", SqlDbType = SqlDbType.SmallInt, Value = execucaoExercicio.repeticoes },
                new SqlParameter() {ParameterName="@pcarga", SqlDbType = SqlDbType.TinyInt, Value = execucaoExercicio.carga },
                new SqlParameter() {ParameterName="@pduracao_descanso", SqlDbType = SqlDbType.SmallInt, Value = execucaoExercicio.duracao_descanso }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
        }

        public List<Execucao_Exercicio> LoadExecucaoExercicios(int treinoTipoId)
        {
            string queryString = "SELECT ee.[execucao_exercicio_id], ee.[exercicio_id], ee.[series], ee.[repeticoes], ee.[carga], ee.[duracao_descanso]" +
                                  " FROM Execucao_Exercicio as ee" +
                                  //" INNER JOIN Exercicio as e ON (ee.exercicio_id = e.exercicio_id)" +
                                  " WHERE ee.[treino_tipo_id] = @ptreino_tipo_id";

            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@ptreino_tipo_id", SqlDbType = SqlDbType.Int, Value = treinoTipoId },
            };

            return DBConnection.SelectQuery<Execucao_Exercicio>(queryString, parametersList);
        }

        public bool RemoveSelectedExercicio(int execExercicioId)
        {
            string queryString = "DELETE FROM Execucao_Exercicio" +
                                           " WHERE [execucao_exercicio_id] = @pexecucao_exercicio_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@pexecucao_exercicio_id", SqlDbType = SqlDbType.Int, Value = execExercicioId }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
        }
    }
}