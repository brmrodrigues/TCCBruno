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

namespace TCCBruno
{
    public static class DBConnection
    {
        public static string ConnectionString
        {
            get
            {
                return "Data Source=tcp:tccbruno.database.windows.net,1433;" +
                        "Initial Catalog=Tcc;User ID=tccbruno@tccbruno;Password=dmXrsYc2";
            }
        }
    }
}