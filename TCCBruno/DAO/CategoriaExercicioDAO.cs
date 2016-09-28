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
            string queryString = "SELECT [categoria_exercicio_id], [nome_categoria_exercicio]" +
                                  " FROM Categoria_Exercicio";
            List<SqlParameter> parametersList = new List<SqlParameter>() { };

            return DBConnection.SelectQuery<Categoria_Exercicio>(queryString, parametersList);
        }
    }
}