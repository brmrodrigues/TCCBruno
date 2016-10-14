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
    public class InstrutorDAO
    {

        public string GetNomeInstrutor(int instrutorId)
        {

            string queryString = "SELECT P.nome_pessoa" +
                                 " FROM Pessoa as P" +
                                 " INNER JOIN Instrutor as I on (P.pessoa_id = I.pessoa_id)" +
                                 " WHERE I.instrutor_id = @pinstrutor_id";
            List<SqlParameter> parametersList = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@pinstrutor_id", SqlDbType = SqlDbType.Int, Value = instrutorId }
            };

            var instrutor = DBConnection.SelectQuery<Pessoa>(queryString, parametersList);

            //Return First Or Default
            return instrutor[0].nome_pessoa;

        }
    }
}