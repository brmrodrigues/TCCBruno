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
    public class AvaliacaoFisicaListAdapter : BaseAdapter<Avaliacao_Fisica>
    {
        List<Avaliacao_Fisica> _items;
        Activity _context;

        public AvaliacaoFisicaListAdapter(Activity context, List<Avaliacao_Fisica> items) : base()
        {
            _items = items;
            _context = context;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Avaliacao_Fisica this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return _items[position].avaliacao_fisica_id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.MeusAlunosCustomLV, null);
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = "Avalia��o " + (position + 1).ToString();
            view.FindViewById<TextView>(Resource.Id.LV_Text2).Text = "Data: " + Convert.ToDateTime(_items[position].data_avaliacao).ToString("dd/MM/yyyy");

            return view;
        }
    }
}