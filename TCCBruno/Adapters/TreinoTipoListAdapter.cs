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
    public class TreinoTipoListAdapter : BaseAdapter<Treino_Tipo>
    {
        List<Treino_Tipo> _items;
        Activity _context;

        public TreinoTipoListAdapter(Activity context, List<Treino_Tipo> items) : base()
        {
            _items = items;
            _context = context;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Treino_Tipo this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return _items[position].treino_tipo_id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _items[position].treino_tipo_nome;

            return view;
        }
    }
}