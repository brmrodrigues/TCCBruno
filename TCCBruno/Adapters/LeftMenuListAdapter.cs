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

namespace TCCBruno.Adapters
{
    public class LeftMenuListAdapter : BaseAdapter<string>
    {
        string[] _items;
        int[] _itemsImageId;
        Activity _context;

        public LeftMenuListAdapter(Activity context, string[] items, int[] itemsImageId) : base()
        {
            _items = items;
            _itemsImageId = itemsImageId;
            _context = context;
        }

        public override string this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Menu.list_single, null);
            view.FindViewById<TextView>(Resource.Id.imgDesc).Text = _items[position];
            view.FindViewById<ImageView>(Resource.Id.imgIcon).SetImageResource(_itemsImageId[position]);


            return view;
        }
    }
}