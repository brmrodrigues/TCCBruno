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
    public class Execucao_Exercicio
    {
        public int execucao_exercicio_id { get; set; }
        public int exercicio_id { get; set; }
        public int treino_tipo_id { get; set; }
        public Nullable<byte> series { get; set; }
        public Nullable<short> repeticoes { get; set; }
        public Nullable<short> carga { get; set; }
        public Nullable<short> duracao_descanso { get; set; }

        public virtual Exercicio Exercicio { get; set; }
        public virtual Treino_Tipo Treino_Tipo { get; set; }
    }
}
