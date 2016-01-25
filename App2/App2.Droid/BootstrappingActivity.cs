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
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/icon")]
    public class BootstrappingActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (GlobalServices.XjtuSite.Account.IsInvalidated)
            {
                DroidUtility.ShowToast(this, "正在载入账户信息……");
                await GlobalServices.XjtuSite.Account.UpdateAsync();
            }
            var intent = new Intent(this, GlobalServices.XjtuSite.Account.IsLoggedIn
                ? typeof (MainActivity)
                : typeof (LoginActivity));
            StartActivity(intent);
            this.Finish();
        }
    }
}