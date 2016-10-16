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
using static AvFisica.Enumeration;

namespace TCCBruno.Extension
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double value)
        {
            return (Math.PI / 180) * value;
        }

        public static int ToInt32(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static decimal ToDecimal(this string value)
        {
            return Convert.ToDecimal(value);
        }

        public static double ToDouble(this string value)
        {
            return value == string.Empty ? 0 : Convert.ToDouble(value);
        }

        public static decimal ToDecimal(this double value)
        {
            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Recebe um EditText e retorna seu conteùdo em Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this EditText value)
        {
            return value.Text == string.Empty ? 0 : Convert.ToDouble(value.Text);
        }

        public static string ToStringNum(this ResultadoClassExercicio value)
        {
            string resultString;
            switch (value)
            {
                case ResultadoClassExercicio.RUIM:
                    resultString = "Ruim";
                    break;
                case ResultadoClassExercicio.ABAIXO_DA_MEDIA:
                    resultString = "Abaixo da Média";
                    break;
                case ResultadoClassExercicio.NA_MEDIA:
                    resultString = "Mediano";
                    break;
                case ResultadoClassExercicio.ACIMA_DA_MEDIA:
                    resultString = "Acima da Média";
                    break;
                case ResultadoClassExercicio.EXCELENTE:
                    resultString = "Excelente";
                    break;
                default:
                    resultString = "N/A";
                    break;
            }
            return resultString;
        }
    }
}