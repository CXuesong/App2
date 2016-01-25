using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using App2.Xjtu;

namespace App2.Droid.Fragments
{
    public class AccountProfileFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override void OnAttach(Activity activity)
        {
            GlobalServices.XjtuSite.Account.Updated += Account_Updated;
            base.OnAttach(activity);
        }

        private void Account_Updated(object sender, EventArgs e)
        {
            UpdateView();
        }

        public override void OnDetach()
        {
            GlobalServices.XjtuSite.Account.Updated += Account_Updated;
            base.OnDetach();
        }

        private void UpdateView(View view = null)
        {
            if (view == null) view = this.View;
            if (view == null) return;
            var userNameView = view.FindViewById<TextView>(Resource.Id.userNameView);
            userNameView.Text = GlobalServices.XjtuSite.Account.UserName;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.AccountProfile, container, false);
            var logoutButton = view.FindViewById<Button>(Resource.Id.logoutButton);
            logoutButton.Click += async (_, e) =>
            {
                logoutButton.Enabled = false;
                try
                {
                    await GlobalServices.XjtuSite.Account.LogoutAsync();
                    DroidUtility.ShowToast(Activity, "×¢Ïú³É¹¦¡£");
                }
                catch (Exception ex)
                {
                    DroidUtility.ReportException(Activity, ex);
                }
                finally
                {
                    logoutButton.Enabled = true;
                }
            };
            UpdateView(view);
            return view;
        }
    }
}