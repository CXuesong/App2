using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using App2.Droid.Fragments;

namespace App2.Droid
{
    [Activity(Label = "Login", Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        private EditText userNameView;
        private EditText passwordView;
        private Button loginButton;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            GlobalServices.XjtuSite.Account.IsLoggedInChanged += Account_IsLoggedInChanged;
            if (!await UpdateAccountStatus())
            {
                userNameView = FindViewById<EditText>(Resource.Id.accountNameEdit);
                passwordView = FindViewById<EditText>(Resource.Id.passwordEdit);
                loginButton = FindViewById<Button>(Resource.Id.loginButton);
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
                        await GlobalServices.XjtuSite.Account.LoginAsync(userNameView.Text, passwordView.Text);
                        GlobalServices.SaveState();
                        DroidUtility.ShowToast(this, "登录成功。");
                    }
                    catch (Exception ex)
                    {
                        DroidUtility.ReportException(this, ex);
                    }
                    finally
                    {
                        loginButton.Enabled = true;
                    }
                };
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GlobalServices.XjtuSite.Account.IsLoggedInChanged -= Account_IsLoggedInChanged;
        }

        private async void Account_IsLoggedInChanged(object sender, EventArgs e)
        {
            await UpdateAccountStatus();
        }

        public async Task<bool> UpdateAccountStatus()
        {
            if (GlobalServices.XjtuSite.Account.IsInvalidated)
            {
                DroidUtility.ShowToast(this, "正在查询登录状态……");
                await GlobalServices.XjtuSite.Account.UpdateAsync();
            }
            if (GlobalServices.XjtuSite.Account.IsLoggedIn)
            {
                var intent = new Intent(this, typeof (MainActivity));
                StartActivity(intent);
                this.Finish();
                return true;
            }
            return false;
        }
    }
}