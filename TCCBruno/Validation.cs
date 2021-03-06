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

namespace TCCBruno
{
    public static class Validation
    {
        public static bool ValidatedFields(GridLayout layout)
        {
            for (int i = 0; i < layout.ChildCount; i++)
            {
                if (layout.GetChildAt(i) is EditText)
                {
                    if (string.IsNullOrWhiteSpace(((EditText)layout.GetChildAt(i)).Text))
                    {
                        //DisplayAlertMessage("Preencha os campos Login e Senha");
                        //fieldText = (EditText)layout.GetChildAt(i);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool ValidatedFields(LinearLayout layout)
        {
            for (int i = 0; i < layout.ChildCount; i++)
            {
                if (layout.GetChildAt(i) is EditText)
                {
                    if (string.IsNullOrWhiteSpace(((EditText)layout.GetChildAt(i)).Text))
                    {
                        //DisplayAlertMessage("Preencha os campos Login e Senha");
                        return false;
                    }
                }
            }
            return true;
        }

        public static void EraseEditTexts(TableLayout layout)
        {
            for (int i = 0; i < layout.ChildCount; i++)
            {
                if (layout.GetChildAt(i) is TableRow)
                {
                    var lay = (TableRow)layout.GetChildAt(i);
                    for (int j = 0; j < lay.ChildCount; j++)
                    {
                        if (lay.GetChildAt(j) is EditText)
                        {
                            ((EditText)lay.GetChildAt(j)).Text = "";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Pop up de um alerta ao usu�rio
        /// </summary>
        /// <param name="message"></param>
        public static void DisplayAlertMessage(string message, Context context)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetMessage(message);
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        //public static void TreinoTipoItemLongClick_DisplayDialog(string nomeTreinoTipo, Context context)
        //{
        //    var builder = new Dialog(context);
        //    builder.SetContentView(Resource.Layout.)
        //    //var builder = new AlertDialog.Builder(context);
        //    //builder.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
        //    //builder.SetTitle("Sub-Treino " + nomeTreinoTipo);
        //    //builder.SetButt
        //}

    }
}