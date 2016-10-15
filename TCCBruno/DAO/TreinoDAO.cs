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
            string queryString = "SELECT [treino_id], [data_inicio], [data_fim]" +
                                    " FROM Treino" +
                                    " WHERE [aluno_id] = @paluno_id";

            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@paluno_id", SqlDbType = SqlDbType.Int, Value = alunoId }
            };

            return DBConnection.SelectQuery<Treino>(queryString, parametersList);
        }

        public bool InsertTreino(Treino treino)
        {

            string queryString = "INSERT INTO [dbo].[Treino] ([aluno_id], [data_inicio], [data_fim])" +
                                    " VALUES (@paluno_id, @pdata_inicio, @pdata_fim)";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@paluno_id", SqlDbType = SqlDbType.Int, Value = treino.aluno_id },
                new SqlParameter() {ParameterName="@pdata_inicio", SqlDbType = SqlDbType.DateTime, Value = treino.data_inicio },
                new SqlParameter() {ParameterName="@pdata_fim", SqlDbType = SqlDbType.DateTime, Value = treino.data_fim }
            };

            return DBConnection.InsertQuery(queryString, parametersList);
        }

        public bool RemoveTreino(int treinoId)
        {
            string queryString = "DELETE FROM Treino" +
                                            " WHERE [treino_id] = @ptreino_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@ptreino_id", SqlDbType = SqlDbType.Int, Value = treinoId }
            };

            return DBConnection.RemoveQuery(queryString, parametersList);
        }
    }
}