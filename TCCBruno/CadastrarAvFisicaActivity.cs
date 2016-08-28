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
using GalaSoft.MvvmLight.Views;

namespace TCCBruno
{
    [Activity(Label = "CadastrarAvFisica")]
    public class CadastrarAvFisicaActivity : ActivityBase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Page2);
            // Create your application here
        }
    }
}