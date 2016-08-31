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

        /// <summary>
        /// Pop up de um alerta ao usuário
        /// </summary>
        /// <param name="message"></param>
        public static void DisplayAlertMessage(string message, Context context)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetMessage(message);
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}