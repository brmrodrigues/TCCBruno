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
using TCCBruno.Model;

namespace TCCBruno.Adapters
{
    public class Execucao_ExerciciosListAdapter : BaseAdapter<Execucao_Exercicio>
    {
        List<Execucao_Exercicio> _items;
        Activity _context;

        public Execucao_ExerciciosListAdapter(Activity context, List<Execucao_Exercicio> items) : base()
        {
            _items = items;
            _context = context;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Execucao_Exercicio this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return _items[position].execucao_exercicio_id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Mostra na lista apenas se o Exercicio do ExercicioExecucao foi carregado
            if (_items[position].Exercicio == null)
                return null;

            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.ExecucaoExercicioCustomLV, null);
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = _items[position].Exercicio.nome_exercicio;
            view.FindViewById<TextView>(Resource.Id.LV_Text2).Text = "S�ries: " + _items[position].series.ToString() +
                                                                     "  Repeti��es: " + _items[position].repeticoes.ToString() +
                                                                     "\nCarga: " + _items[position].carga.ToString() + " Kg" +
                                                                     "  Intervalo: " + _items[position].duracao_descanso.ToString() + " s";
            //view.FindViewById<Switch>(Resource.Id.SW_ExecucaoExercicio).Checked = false;

            return view;
        }

        public string GetNomeExercicio(int position)
        {
            return _items[position].Exercicio.nome_exercicio;
        }
    }
}