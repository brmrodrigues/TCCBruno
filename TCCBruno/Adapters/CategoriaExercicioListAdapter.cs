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
    public class CategoriaExercicioListAdapter : BaseAdapter<Categoria_Exercicio>
    {
        List<Categoria_Exercicio> _items;
        Activity _context;

        public CategoriaExercicioListAdapter(Activity context, List<Categoria_Exercicio> items) : base()
        {
            _items = items;
            _context = context;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override Categoria_Exercicio this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return _items[position].categoria_exercicio_id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.CategoriaCustomLV, null);
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = _items[position].nome_categoria_exercicio;

            return view;
        }
    }
}