//#define ALLOW_PASSWORD_PERSISTENCE

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
    public class LoginActivity : MyActivity
    {
        private EditText userNameView;
        private EditText passwordView;
        private Button loginButton;
        private CheckBox savePasswordCheckBox;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            GlobalServices.XjtuSite.Account.IsLoggedInChanged += Account_IsLoggedInChanged;
            if (!await UpdateAccountStatus())
            {
                userNameView = FindViewById<EditText>(Resource.Id.accountNameEdit);
                passwordView = FindViewById<EditText>(Resource.Id.passwordEdit);
                savePasswordCheckBox = FindViewById<CheckBox>(Resource.Id.savePasswordCheckbox);
                loginButton = FindViewById<Button>(Resource.Id.loginButton);
                //载入设置。
                using (var pref = GetPreferences(FileCreationMode.Private))
                {
                    userNameView.Text = pref.GetString("userName", "");
                    passwordView.Text = pref.GetString("password", "");
                    savePasswordCheckBox.Checked = pref.GetBoolean("savePassword", false);
                }
                if (!string.IsNullOrWhiteSpace(userNameView.Text)) passwordView.RequestFocus();
                //侦听事件。
                EventHandler<TextChangedEventArgs> userNamePasswordChanged = (_, e) =>
                {
                    loginButton.Enabled = !string.IsNullOrWhiteSpace(userNameView.Text) &&
                                          !string.IsNullOrEmpty(passwordView.Text);
                };
                userNameView.TextChanged += userNamePasswordChanged;
                passwordView.TextChanged += userNamePasswordChanged;
                savePasswordCheckBox.CheckedChange += SavePasswordCheckBox_CheckedChange;
                loginButton.Click += LoginButton_Click;
            }
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            loginButton.Enabled = false;
            try
            {
                await GlobalServices.XjtuSite.Account.LoginAsync(userNameView.Text, passwordView.Text);
                // 保存设置。
                using (var pref = GetPreferences(FileCreationMode.Private))
                {
                    var edit = pref.Edit();
                    var savePassword = savePasswordCheckBox.Checked;
                    edit.PutString("userName", userNameView.Text);
                    edit.PutString("password", savePassword ? passwordView.Text : "");
                    edit.PutBoolean("savePassword", savePassword);
                    edit.Commit();
                }
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
        }

        private void SavePasswordCheckBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
#if ALLOW_PASSWORD_PERSISTENCE
            if (e.IsChecked)
            {
                new AlertDialog.Builder(this)
                    .SetTitle(Resource.String.safety_warning)
                    .SetMessage(Resource.String.safety_warning_password)
                    .SetPositiveButton(Resource.String.i_accept, (_, e1) => { })
                    .SetNegativeButton(Resource.String.i_decline, (_, e1) => { savePasswordCheckBox.Checked = false; })
                    .Show();
            }
#else
            if (e.IsChecked)
            {
                savePasswordCheckBox.Checked = false;
                new AlertDialog.Builder(this)
                    .SetTitle(Resource.String.safety_warning)
                    .SetMessage(Resource.String.safety_warning_no_password_persistence)
                    .Show();
            }
#endif
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