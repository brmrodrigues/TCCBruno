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

        public bool InsertTreino_Tipo(Treino_Tipo newTreino_Tipo)
        {
            string queryString = "INSERT INTO [dbo].[Treino_Tipo] ([treino_id], [treino_tipo_nome], [duracao], [descricao])" +
                                    " VALUES (@ptreino_id, @ptreino_tipo_nome, @pduracao, @pdescricao)";

            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@ptreino_id", SqlDbType = SqlDbType.Int, Value = newTreino_Tipo.treino_id },
                new SqlParameter() {ParameterName="@ptreino_tipo_nome", SqlDbType = SqlDbType.VarChar, Size=300, Value = newTreino_Tipo.treino_tipo_nome },
                new SqlParameter() {ParameterName="@pduracao", SqlDbType = SqlDbType.Float, Value = newTreino_Tipo.duracao },
                new SqlParameter() {ParameterName="@pdescricao", SqlDbType = SqlDbType.VarChar, Size = -1, Value = newTreino_Tipo.descricao }

            };

            if (DBConnection.ExecuteNonQuery(queryString, parametersList))
            {
                return true;
            }

            return false;
        }


        public List<Treino_Tipo> LoadTreino_Tipos(int treinoId)
        {
            //SqlConnection connection;
            //using (connection = new SqlConnection(DBConnection.ConnectionString))
            //{
            string queryString = "SELECT [treino_tipo_id], [treino_tipo_nome], [duracao]" +
                                    " FROM Treino_Tipo" +
                                    " WHERE [treino_id] = @ptreino_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName="@ptreino_id", SqlDbType = SqlDbType.Int, Value = treinoId }
                };

            return DBConnection.SelectQuery<Treino_Tipo>(queryString, parametersList);
            //}

        }

        public List<Treino_Tipo> LoadTreino_TiposAtual(int alunoId)
        {
            string queryString = "SELECT DISTINCT [treino_tipo_id], [treino_tipo_nome]" +
                                    " FROM Treino_Tipo AS tipo" +
                                    " INNER JOIN Treino AS treino on (tipo.treino_id = treino.treino_id)" +
                                    " INNER JOIN Aluno AS aluno on (treino.aluno_id = aluno.aluno_id)" +
                                    " WHERE aluno.aluno_id = @alunoId AND @pdataAtual BETWEEN treino.data_inicio AND treino.data_fim";

            List<SqlParameter> parametersList = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName="@alunoId", SqlDbType = SqlDbType.Int, Value = alunoId },
                    new SqlParameter() {ParameterName="@pdataAtual", SqlDbType = SqlDbType.SmallDateTime, Value = DateTime.Now }
                };

            return DBConnection.SelectQuery<Treino_Tipo>(queryString, parametersList);
        }

        public bool RemoveTreinoTipo(int treinoTipoId)
        {
            string queryString = "DELETE FROM Treino_Tipo" +
                                            " WHERE [treino_tipo_id] = @ptreino_tipo_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@ptreino_tipo_id", SqlDbType = SqlDbType.Int, Value = treinoTipoId }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
        }

        public bool UpdateNomeSubTreino(int subTreinoId, string newNomeSubTreino)
        {
            string queryString = "UPDATE Treino_Tipo" +
                                    " SET treino_tipo_nome = @treino_tipo_nome" +
                                    " WHERE treino_tipo_id = @treino_tipo_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@treino_tipo_nome", SqlDbType = SqlDbType.VarChar, Value = newNomeSubTreino },
                new SqlParameter() {ParameterName="@treino_tipo_id", SqlDbType = SqlDbType.Int, Value = subTreinoId }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);



        }
    }
}