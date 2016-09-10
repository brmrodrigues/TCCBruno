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
    public class MeusAlunosPageAdapter : BaseAdapter<Pessoa>
    {

        List<Pessoa> _items;
        Activity _context;

        public MeusAlunosPageAdapter(Activity context, List<Pessoa> items) : base()
        {
            this._context = context;
            this._items = items;
        }

        public override Pessoa this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override long GetItemId(int position)
        {
            return _items[position].aluno_id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Resource.Layout.MeusAlunosCustomLV, null);
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = _items[position].nome_pessoa;
            view.FindViewById<TextView>(Resource.Id.LV_Text2).Text = _items[position].status ? "Ativo" : "Inativo";


            return view;
        }
    }
}