using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using App2.Xjtu;

namespace App2.Droid.Fragments
{
    public class LoginFragment : Fragment
    {
        private AccountManager account;
        private EditText userNameView;
        private EditText passwordView;
        private Button loginButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override void OnAttach(Activity activity)
        {
            account = ((IServiceProvider)activity).RequireService<AccountManager>();
            base.OnAttach(activity);
        }

        public override void OnDetach()
        {
            base.OnDetach();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Login, container, false);
            userNameView = view.FindViewById<EditText>(Resource.Id.accountNameEdit);
            passwordView = view.FindViewById<EditText>(Resource.Id.passwordEdit);
            loginButton = view.FindViewById<Button>(Resource.Id.loginButton);
            EventHandler<TextChangedEventArgs> userNamePasswordChanged = (_, e) =>
            {
                loginButton.Enabled = !string.IsNullOrWhiteSpace(userNameView.Text) &&
                                      !string.IsNullOrEmpty(passwordView.Text);
            };
            userNameView.TextChanged += userNamePasswordChanged;
            passwordView.TextChanged += userNamePasswordChanged;
            loginButton.Click += async (_, e) =>
            {
                loginButton.Enabled = false;
                try
                {
                    await account.LoginAsync(userNameView.Text, passwordView.Text);
                    DroidUtility.ShowToast(Activity, "µÇÂ¼³É¹¦¡£");
                }
                catch (Exception ex)
                {
                    DroidUtility.ReportException(Activity, ex);
                }
                finally
                {
                    loginButton.Enabled = true;
                }
            };
            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}