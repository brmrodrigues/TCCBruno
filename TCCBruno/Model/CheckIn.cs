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

namespace TCCBruno.Model
{
    public class CheckIn
    {
        public int checkin_id { get; set; }
        public int aluno_id { get; set; }
        public int treino_tipo_id { get; set; }
        public DateTime data_checkin { get; set; }

        public virtual Aluno Aluno { get; set; }
        public virtual Treino_Tipo Treino_Tipo { get; set; }
    }
}