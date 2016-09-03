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
using System.Collections.ObjectModel;

namespace TCCBruno.Model
{
    public class Categoria_Exercicio
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categoria_Exercicio()
        {
            //this.Exercicio = new ObservableCollection<Exercicio>();
        }

        public int categoria_exercicio_id { get; set; }
        public string nome_categoria_exercicio { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ObservableCollection<Exercicio> Exercicio { get; set; }
    }
}