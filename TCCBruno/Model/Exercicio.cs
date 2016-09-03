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
    public class Exercicio
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exercicio()
        {
            //this.Execucao_Exercicio = new HashSet<Execucao_Exercicio>();
        }

        public int exercicio_id { get; set; }
        public int categoria_exercicio_id { get; set; }
        public string nome_exercicio { get; set; }
        public string descricao { get; set; }

        //public virtual Categoria_Exercicio Categoria_Exercicio { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Execucao_Exercicio> Execucao_Exercicio { get; set; }
    }
}