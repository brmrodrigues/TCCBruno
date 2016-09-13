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
    public class TreinosAdapter : BaseAdapter<Treino>
    {

        List<Treino> _items;
        Activity _context;
        int treinoCount = 1;

        public TreinosAdapter(Activity context, List<Treino> items) : base()
        {
            this._context = context;
            this._items = items;
        }

        public override Treino this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override long GetItemId(int position)
        {
            return _items[position].treino_id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Resource.Layout.MeusAlunosCustomLV, null);
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = "Treino " + treinoCount++;
            view.FindViewById<TextView>(Resource.Id.LV_Text2).Text = "De: " +
                                            _items[position].data_inicio.ToString("dd/MM/yyyy") +
                                            "   Até: " + _items[position].data_fim.ToString("dd/MM/yyyy");


            return view;
        }
    }
}