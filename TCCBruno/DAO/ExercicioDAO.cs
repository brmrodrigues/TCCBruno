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
            string queryString = "SELECT [exercicio_id], [nome_exercicio], [descricao]" +
                                  " FROM Exercicio" +
                                  " WHERE [categoria_exercicio_id] = @pcategoria_exercicio_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@pcategoria_exercicio_id", SqlDbType = SqlDbType.Int, Value = categoriaId }
            };

            return DBConnection.SelectQuery<Exercicio>(queryString, parametersList);
        }

        public Exercicio GetExercicio(int exercicioId)
        {
            string queryString = "SELECT [exercicio_id], [nome_exercicio], [descricao]" +
                                  " FROM Exercicio" +
                                  " WHERE [exercicio_id] = @pexercicio_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@pexercicio_id", SqlDbType = SqlDbType.Int, Value = exercicioId }
            };

            return DBConnection.SelectQuery<Exercicio>(queryString, parametersList).FirstOrDefault(x => x.exercicio_id == exercicioId);
        }

    }
}