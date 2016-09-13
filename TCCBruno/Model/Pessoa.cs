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
using Java.Lang;
using System.Collections.ObjectModel;

namespace TCCBruno.Model
{
    public class Pessoa
    {
        #region Essas colunas NÃO são da tabela Pessoa, mas estão aqui como resultado da query para carregar na ListView
        //public int aluno_id { get; set; }
        //public int instrutor_id { get; set; }
        #endregion
        public int pessoa_id { get; set; }
        public string nome_pessoa { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public bool status { get; set; }


        //public virtual ObservableCollection<Aluno> Aluno { get; set; }
        //public virtual ICollection<Instrutor> Instrutor { get; set; }

    }
}