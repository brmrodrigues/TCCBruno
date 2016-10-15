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
using Microsoft.Practices.ServiceLocation;
using TCCBruno.Adapters;
using TCCBruno.DAO;

namespace TCCBruno
{
    public class LogOutFragment : Android.Support.V4.App.Fragment
    {
        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LogOutPage, container, false);
            view.FindViewById<Button>(Resource.Id.BTN_LogOut).Click += BTN_LogOut_Click;

            return view;
        }

        private void BTN_LogOut_Click(object sender, EventArgs e)
        {
            //Navegar para a página de Login, mas antes devemos limpar a Stack do GoBack button
            var intent = new Intent(this.Activity, typeof(LoginActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);

            StartActivity(intent);
            this.Activity.Finish();
        }
    }
}