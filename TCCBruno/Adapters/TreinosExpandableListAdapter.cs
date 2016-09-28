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
using TCCBruno.Model;

namespace TCCBruno.Adapters
{
    public class TreinosExpandableListAdapter : BaseExpandableListAdapter
    {
        readonly Activity _context;
        List<Treino> _items;
        //int treinoCount = 1;

        public TreinosExpandableListAdapter(Activity context, List<Treino> items) : base()
        {
            _context = context;
            _items = items;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = _context.LayoutInflater.Inflate(Resource.Layout.MeusAlunosCustomLV, null);
            }
            header.FindViewById<TextView>(Resource.Id.LV_Text1).Text = "Treino " + (groupPosition + 1).ToString();
            header.FindViewById<TextView>(Resource.Id.LV_Text2).Text = "De: " +
                                            DateTime.Parse(_items[groupPosition].data_inicio).ToString("dd/MM/yyyy") +
                                            "   Até: " + DateTime.Parse(_items[groupPosition].data_fim).ToString("dd/MM/yyyy");

            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View child = convertView;
            if (child == null)
            {
                child = _context.LayoutInflater.Inflate(Resource.Layout.MeusAlunosCustomLV, null);
            }
            //string newId = "", newValue = "";
            //GetChildViewHelper(groupPosition, childPosition, out newId, out newValue);
            child.FindViewById<TextView>(Resource.Id.LV_Text1).Text = _items[groupPosition].Treino_Tipo[childPosition].treino_tipo_nome;
            child.FindViewById<TextView>(Resource.Id.LV_Text2).Text = "Duração: " +
                                                                    _items[groupPosition].Treino_Tipo[childPosition].duracao.ToString() + " Horas";
            return child;
        }

        public override int GroupCount
        {
            get
            {
                return _items.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public string GetNomeTreinoTipo(int groupPosition, int childPosition)
        {
            return _items[groupPosition].Treino_Tipo[childPosition].treino_tipo_nome;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return _items[groupPosition].Treino_Tipo[childPosition].treino_tipo_id;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var treinoTipo = _items[groupPosition].Treino_Tipo;
            return treinoTipo == null ? 0 : treinoTipo.Count;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return _items[groupPosition].treino_id;
        }


        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}