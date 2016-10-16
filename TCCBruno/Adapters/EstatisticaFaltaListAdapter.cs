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
    public class EstatisticaFaltaListAdapter : BaseAdapter<ViewPresencaSubTreino>
    {
        List<ViewPresencaSubTreino> _items;
        Activity _context;

        public EstatisticaFaltaListAdapter(Activity context, List<ViewPresencaSubTreino> items) : base()
        {
            _items = items;
            _context = context;
        }

        public override ViewPresencaSubTreino this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.MeusAlunosCustomLV, null);
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = "SubTreino " + _items[position].NomeSubTreino;
            view.FindViewById<TextView>(Resource.Id.LV_Text2).Text = "Descrição: " + _items[position].Descricao +
                                                                    "\nNro de Presenças: " + _items[position].PresencaCount.ToString();


            return view;
        }
    }
}