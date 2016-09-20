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
    public class Treino_Tipo
    {
        public int treino_tipo_id { get; set; }
        public int treino_id { get; set; }
        public string treino_tipo_nome { get; set; }
        public double duracao { get; set; }
        public string descricao { get; set; }
    }
}