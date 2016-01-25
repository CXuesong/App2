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
        private AccountManager account;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override void OnAttach(Activity activity)
        {
            account = ((IServiceProvider)activity).RequireService<AccountManager>();
            account.Updated += Account_Updated;
            base.OnAttach(activity);
        }

        private void Account_Updated(object sender, EventArgs e)
        {
            UpdateView();
        }

        public override void OnDetach()
        {
            account.Updated += Account_Updated;
            base.OnDetach();
        }

        private void UpdateView(View view = null)
        {
            if (view == null) view = this.View;
            if (view == null) return;
            var userNameView = view.FindViewById<TextView>(Resource.Id.userNameView);
            userNameView.Text = account.UserName;
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
                    await account.LogoutAsync();
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