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
    public class Treino
    {
        public int treino_id { get; set; }
        public int aluno_id { get; set; }
        public string nome_treino { get; set; }
        public string data_inicio { get; set; }
        public string data_fim { get; set; }

        public virtual Aluno Aluno { get; set; }
        public virtual List<Treino_Tipo> Treino_Tipo { get; set; }
    }
}