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
    }
}