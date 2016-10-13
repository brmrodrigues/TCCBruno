using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;

namespace TCCBruno
{
    public interface IActionBarActivity
    {

        void SetSupportActionBar(SupportToolbar toolbar);
    }
}