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

namespace TCCBruno.Model
{
    public class Instrutor
    {

        public int instrutor_id { get; set; }
        public int pessoa_id { get; set; }

        public virtual List<Aluno> Aluno { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}