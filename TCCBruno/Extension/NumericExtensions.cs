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
            return Convert.ToDouble(value);
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
    }
}