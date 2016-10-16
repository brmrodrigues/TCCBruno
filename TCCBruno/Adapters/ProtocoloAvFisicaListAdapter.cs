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
    public class ProtocoloAvFisicaListAdapter : BaseAdapter<string>
    {
        List<string> _items;
        Activity _context;

        public ProtocoloAvFisicaListAdapter(Activity context) : base()
        {
            _items = new List<string>();
            _items.Add("GUEDES 3DC");
            _items.Add("JACKSON & POLLOCK 3DC");
            _items.Add("JACKSON & POLLOCK 7DC");
            _context = context;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override string this[int position]
        {
            get { return _items[position]; }
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
            view.FindViewById<TextView>(Resource.Id.LV_Text1).Text = _items[position];

            return view;
        }
    }
}