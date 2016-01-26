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

namespace App2.Droid
{
    public class MyActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();
            GlobalServices.CurrentActivity = this;
        }
    }
}