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
    public class Avaliacao_FisicaDAO
    {

        public List<Avaliacao_Fisica> LoadAvaliacoesResumo(int alunoId)
        {
            string queryString = "SELECT [avaliacao_fisica_id], [data_avaliacao]" +
                                    " FROM Avaliacao_Fisica" +
                                    " WHERE [aluno_id] = @paluno_id";

            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@paluno_id", SqlDbType = SqlDbType.Int, Value = alunoId }
            };

            return DBConnection.SelectQuery<Avaliacao_Fisica>(queryString, parametersList);
        }

        public bool InsertAvaliacao(Avaliacao_Fisica avaliacao)
        {
            #region InsertAvFisicaQuery
            string queryString = "INSERT INTO Avaliacao_Fisica" +
           " ([aluno_id]" +
           ",[data_avaliacao]" +
           ",[estatura]" +
           ",[pressao_arterial]" +
           ",[torax]" +
           ",[cintura]" +
           ",[abdome]" +
           ",[braco_dir]" +
           ",[braco_esq]" + ")" +
           //",[antebr_dir]" +
           //",[antebr_esq]" +
           //",[quadril]" + 
           //",[coxa_dir]" +
           //",[coxa_esq]" +
           //",[perna_dir]" +
           //",[perna_esq]" +
           //",[ombro]" +
           //",[tricipital]" +
           //",[subescapular]" +
           //",[suprailicia]" +
           //",[abdominal]" +
           //",[peitoral]" +
           //",[axilar_media]" +
           //",[coxa_medial]" +
           //",[radio_u]" +
           //",[femural]" +
           //",[flex_geral1]" +
           //",[flex_geral2]" +
           //",[flex_geral3]" +
           //",[flex_braco]" +
           //",[abdominais_rep])" +
           " OUTPUT Inserted.avaliacao_fisica_id " +
           " VALUES " +
           "(@aluno_id " +
           ",@data_avaliacao" +
            ",@estatura" +
            ",@pressao_arterial" +
            ",@torax" +
            ",@cintura" +
            ",@abdome" +
            ",@braco_dir" +
            ",@braco_esq" + ")";
            //",@antebr_dir" +
            //",@antebr_esq" + 
            //",@quadril" + 
            //",@coxa_dir" +
            //",@coxa_esq" +
            //",@perna_dir" +
            //",@perna_esq" +
            //",@ombro" +
            //",@tricipital" +
            //",@subescapular" +
            //",@suprailicia" +
            //",@abdominal" +
            //",@peitoral" +
            //",@axilar_media" +
            //",@coxa_medial" +
            //",@radio_u" +
            //",@femural" +
            //",@flex_geral1" +
            //",@flex_geral2" +
            //",@flex_geral3" +
            //",@flex_braco" +
            //",@abdominais_rep)";
            #endregion
            var data_avaliacao = DateTime.Parse(avaliacao.data_avaliacao);
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@aluno_id", SqlDbType = SqlDbType.Int, Value = avaliacao.aluno_id },
                new SqlParameter() {ParameterName="@data_avaliacao", SqlDbType = SqlDbType.DateTime, Value = avaliacao.data_avaliacao },
                new SqlParameter() {ParameterName="@estatura", SqlDbType = SqlDbType.Decimal, Value = avaliacao.estatura, },
                new SqlParameter() {ParameterName="@pressao_arterial", SqlDbType = SqlDbType.Decimal, Value = avaliacao.pressao_arterial, },
                new SqlParameter() {ParameterName="@torax", SqlDbType = SqlDbType.Decimal, Value = avaliacao.torax, },
                new SqlParameter() {ParameterName="@cintura", SqlDbType = SqlDbType.Decimal, Value = avaliacao.cintura, },
                new SqlParameter() {ParameterName="@abdome", SqlDbType = SqlDbType.Decimal, Value = avaliacao.abdome, },
                new SqlParameter() {ParameterName="@braco_dir", SqlDbType = SqlDbType.Decimal, Value = avaliacao.braco_dir, },
                new SqlParameter() {ParameterName="@braco_esq", SqlDbType = SqlDbType.Decimal, Value = avaliacao.braco_esq, }
            };
            int avFisicaId = -1;
            SqlConnection connection;
            using (connection = new SqlConnection(DBConnection.ConnectionString))
            {

                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                    sqlCommand.Parameters.AddRange(parametersList.ToArray());
                    sqlCommand.CommandType = CommandType.Text;

                    avFisicaId = (int)sqlCommand.ExecuteScalar();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            if (avFisicaId != -1)
                return InsertAvaliacaoPage2(avFisicaId, avaliacao);
            //return UpdateAvaliacao(avFisicaId, avaliacao);
            else
                return false;
        }



        public bool InsertAvaliacaoPage2(int avaliacaoFisicaId, Avaliacao_Fisica avaliacao)
        {
            string queryString = "UPDATE Avaliacao_Fisica" +
                                    " SET antebr_dir = @antebr_dir," +
                                    " antebr_esq = @antebr_esq," +
                                    " quadril = @quadril," +
                                    " coxa_dir = @coxa_dir," +
                                    " coxa_esq = @coxa_esq," +
                                    " perna_dir = @perna_dir," +
                                    " perna_esq = @perna_esq," +
                                    " ombro = @ombro" +
                                    " WHERE avaliacao_fisica_id = @avaliacao_fisica_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@avaliacao_fisica_id", SqlDbType = SqlDbType.Int, Value = avaliacaoFisicaId },
                new SqlParameter() {ParameterName="@antebr_dir", SqlDbType = SqlDbType.Decimal, Value = avaliacao.antebr_dir, },
                new SqlParameter() {ParameterName="@antebr_esq", SqlDbType = SqlDbType.Decimal, Value = avaliacao.antebr_esq, },
                new SqlParameter() {ParameterName="@quadril", SqlDbType = SqlDbType.Decimal, Value = avaliacao.quadril, },
                new SqlParameter() {ParameterName="@coxa_dir", SqlDbType = SqlDbType.VarChar, Value = avaliacao.coxa_dir, },
                new SqlParameter() {ParameterName="@coxa_esq", SqlDbType = SqlDbType.VarChar, Value = avaliacao.coxa_esq, },
                new SqlParameter() {ParameterName="@perna_dir", SqlDbType = SqlDbType.Decimal, Value = avaliacao.perna_dir, },
                new SqlParameter() {ParameterName="@perna_esq", SqlDbType = SqlDbType.Decimal, Value = avaliacao.perna_esq, },
                new SqlParameter() {ParameterName="@ombro", SqlDbType = SqlDbType.Decimal, Value = avaliacao.ombro, }
            };

            if (!DBConnection.ExecuteNonQuery(queryString, parametersList))
            {
                return false;
            }
            return InsertAvaliacaoPage3(avaliacaoFisicaId, avaliacao);
        }


        public bool InsertAvaliacaoPage3(int avaliacaoFisicaId, Avaliacao_Fisica avaliacao)
        {
            string queryString = "UPDATE Avaliacao_Fisica" +
                                    " SET tricipital = @tricipital," +
                                    " subescapular = @subescapular," +
                                    " suprailicia = @suprailicia," +
                                    " abdominal = @abdominal," +
                                    " peitoral = @peitoral," +
                                    " axilar_media = @axilar_media," +
                                    " coxa_medial = @coxa_medial" +
                                    " WHERE avaliacao_fisica_id = @avaliacao_fisica_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@avaliacao_fisica_id", SqlDbType = SqlDbType.Int, Value = avaliacaoFisicaId },
                new SqlParameter() {ParameterName="@tricipital", SqlDbType = SqlDbType.Decimal, Value = avaliacao.tricipital, },
                new SqlParameter() {ParameterName="@subescapular", SqlDbType = SqlDbType.Decimal, Value = avaliacao.subescapular, },
                new SqlParameter() {ParameterName="@suprailicia", SqlDbType = SqlDbType.Decimal, Value = avaliacao.suprailicia, },
                new SqlParameter() {ParameterName="@abdominal", SqlDbType = SqlDbType.Decimal, Value = avaliacao.abdominal, },
                new SqlParameter() {ParameterName="@peitoral", SqlDbType = SqlDbType.Decimal, Value = avaliacao.peitoral, },
                new SqlParameter() {ParameterName="@axilar_media", SqlDbType = SqlDbType.Decimal, Value = avaliacao.axilar_media, },
                new SqlParameter() {ParameterName="@coxa_medial", SqlDbType = SqlDbType.Decimal, Value = avaliacao.coxa_medial, }

            };

            if (!DBConnection.ExecuteNonQuery(queryString, parametersList))
            {
                return false;
            }
            return InsertAvaliacaoPage4(avaliacaoFisicaId, avaliacao);
        }

        public bool InsertAvaliacaoPage4(int avaliacaoFisicaId, Avaliacao_Fisica avaliacao)
        {
            string queryString = "UPDATE Avaliacao_Fisica" +
                                    " SET radio_u = @radio_u," +
                                    " femural = @femural," +
                                    " flex_geral1 = @flex_geral1," +
                                    " flex_geral2 = @flex_geral2," +
                                    " flex_geral3 = @flex_geral3," +
                                    " flex_braco = @flex_braco," +
                                    " abdominais_rep = @abdominais_rep," +
                                    " peso = @peso" +
                                    " WHERE avaliacao_fisica_id = @avaliacao_fisica_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@avaliacao_fisica_id", SqlDbType = SqlDbType.Int, Value = avaliacaoFisicaId },
                new SqlParameter() {ParameterName="@radio_u", SqlDbType = SqlDbType.Decimal, Value = avaliacao.radio_u, },
                new SqlParameter() {ParameterName="@femural", SqlDbType = SqlDbType.Decimal, Value = avaliacao.femural, },
                new SqlParameter() {ParameterName="@flex_geral1", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_geral1, },
                new SqlParameter() {ParameterName="@flex_geral2", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_geral2, },
                new SqlParameter() {ParameterName="@flex_geral3", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_geral3, },
                new SqlParameter() {ParameterName="@flex_braco", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_braco, },
                new SqlParameter() {ParameterName="@abdominais_rep", SqlDbType = SqlDbType.Int, Value = avaliacao.abdominais_rep, },
                new SqlParameter() {ParameterName="@peso", SqlDbType = SqlDbType.Int, Value = avaliacao.peso, }
            };

            if (!DBConnection.ExecuteNonQuery(queryString, parametersList))
            {
                return false;
            }
            return true;
        }

        public bool UpdateAvaliacao(int avaliacaoFisicaId, Avaliacao_Fisica avaliacao)
        {
            string queryString = "UPDATE Avaliacao_Fisica" +
                                    " SET peso = @peso," +
                                    " estatura = @estatura," +
                                    " pressao_arterial = @pressao_arterial," +
                                    " torax = @torax," +
                                    " cintura = @cintura," +
                                    " abdome = @abdome," +
                                    " braco_dir = @braco_dir," +
                                    " braco_esq = @braco_esq," +
                                    " antebr_dir = @antebr_dir," +
                                    " antebr_esq = @antebr_esq," +
                                    " quadril = @quadril," +
                                    " coxa_dir = @coxa_dir," +
                                    " coxa_esq = @coxa_esq," +
                                    " perna_dir = @perna_dir," +
                                    " perna_esq = @perna_esq," +
                                    " ombro = @ombro," +
                                    " tricipital = @tricipital," +
                                    " subescapular = @subescapular," +
                                    " suprailicia = @suprailicia," +
                                    " abdominal = @abdominal," +
                                    " peitoral = @peitoral," +
                                    " axilar_media = @axilar_media," +
                                    " coxa_medial = @coxa_medial," +
                                    " radio_u = @radio_u," +
                                    " femural = @femural," +
                                    " flex_geral1 = @flex_geral1," +
                                    " flex_geral2 = @flex_geral2," +
                                    " flex_geral3 = @flex_geral3," +
                                    " flex_braco = @flex_braco," +
                                    " abdominais_rep = @abdominais_rep" +
                                    " WHERE avaliacao_fisica_id = @avaliacao_fisica_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@avaliacao_fisica_id", SqlDbType = SqlDbType.Int, Value = avaliacaoFisicaId },
                new SqlParameter() {ParameterName="@peso", SqlDbType = SqlDbType.Decimal, Value = avaliacao.peso },
                new SqlParameter() {ParameterName="@estatura", SqlDbType = SqlDbType.Decimal, Value = avaliacao.estatura, },
                new SqlParameter() {ParameterName="@pressao_arterial", SqlDbType = SqlDbType.Decimal, Value = avaliacao.pressao_arterial, },
                new SqlParameter() {ParameterName="@torax", SqlDbType = SqlDbType.Decimal, Value = avaliacao.torax, },
                new SqlParameter() {ParameterName="@cintura", SqlDbType = SqlDbType.Decimal, Value = avaliacao.cintura, },
                new SqlParameter() {ParameterName="@abdome", SqlDbType = SqlDbType.Decimal, Value = avaliacao.abdome, },
                new SqlParameter() {ParameterName="@braco_dir", SqlDbType = SqlDbType.Decimal, Value = avaliacao.braco_dir, },
                new SqlParameter() {ParameterName="@braco_esq", SqlDbType = SqlDbType.Decimal, Value = avaliacao.braco_esq, },
                new SqlParameter() {ParameterName="@antebr_dir", SqlDbType = SqlDbType.Decimal, Value = avaliacao.antebr_dir, },
                new SqlParameter() {ParameterName="@antebr_esq", SqlDbType = SqlDbType.Decimal, Value = avaliacao.antebr_esq, },
                new SqlParameter() {ParameterName="@quadril", SqlDbType = SqlDbType.Decimal, Value = avaliacao.quadril, },
                new SqlParameter() {ParameterName="@coxa_dir", SqlDbType = SqlDbType.VarChar, Value = avaliacao.coxa_dir, },
                new SqlParameter() {ParameterName="@coxa_esq", SqlDbType = SqlDbType.VarChar, Value = avaliacao.coxa_esq, },
                new SqlParameter() {ParameterName="@perna_dir", SqlDbType = SqlDbType.Decimal, Value = avaliacao.perna_dir, },
                new SqlParameter() {ParameterName="@perna_esq", SqlDbType = SqlDbType.Decimal, Value = avaliacao.perna_esq, },
                new SqlParameter() {ParameterName="@ombro", SqlDbType = SqlDbType.Decimal, Value = avaliacao.ombro, },
                new SqlParameter() {ParameterName="@tricipital", SqlDbType = SqlDbType.Decimal, Value = avaliacao.tricipital, },
                new SqlParameter() {ParameterName="@subescapular", SqlDbType = SqlDbType.Decimal, Value = avaliacao.subescapular, },
                new SqlParameter() {ParameterName="@suprailicia", SqlDbType = SqlDbType.Decimal, Value = avaliacao.suprailicia, },
                new SqlParameter() {ParameterName="@abdominal", SqlDbType = SqlDbType.Decimal, Value = avaliacao.abdominal, },
                new SqlParameter() {ParameterName="@peitoral", SqlDbType = SqlDbType.Decimal, Value = avaliacao.peitoral, },
                new SqlParameter() {ParameterName="@axilar_media", SqlDbType = SqlDbType.Decimal, Value = avaliacao.axilar_media, },
                new SqlParameter() {ParameterName="@coxa_medial", SqlDbType = SqlDbType.Decimal, Value = avaliacao.coxa_medial, },
                new SqlParameter() {ParameterName="@radio_u", SqlDbType = SqlDbType.Decimal, Value = avaliacao.radio_u, },
                new SqlParameter() {ParameterName="@femural", SqlDbType = SqlDbType.Decimal, Value = avaliacao.femural, },
                new SqlParameter() {ParameterName="@flex_geral1", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_geral1, },
                new SqlParameter() {ParameterName="@flex_geral2", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_geral2, },
                new SqlParameter() {ParameterName="@flex_geral3", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_geral3, },
                new SqlParameter() {ParameterName="@flex_braco", SqlDbType = SqlDbType.Int, Value = avaliacao.flex_braco, },
                new SqlParameter() {ParameterName="@abdominais_rep", SqlDbType = SqlDbType.Int, Value = avaliacao.abdominais_rep, }
            };

            if (!DBConnection.ExecuteNonQuery(queryString, parametersList))
            {
                return false;
            }
            return true;
            //return InsertAvaliacaoPage3(avaliacaoFisicaId, avaliacao);
        }

        public bool RemoveAvaliacaoFisica(int avFisicaId)
        {
            string queryString = "DELETE FROM Avaliacao_Fisica" +
                                            " WHERE [avaliacao_fisica_id] = @avaliacao_fisica_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@avaliacao_fisica_id", SqlDbType = SqlDbType.Int, Value = avFisicaId }
            };

            return DBConnection.ExecuteNonQuery(queryString, parametersList);
        }
    }
}