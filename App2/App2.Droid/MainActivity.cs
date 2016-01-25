using System;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using App2.Droid.Fragments;
using App2.Xjtu;
using Java.IO;

namespace App2.Droid
{
	[Activity (Label = "App2.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private ActionBar.Tab AddTab(string title, EventHandler<ActionBar.TabEventArgs> onTabSelected)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(title);
            tab.TabSelected += onTabSelected;
            ActionBar.AddTab(tab);
            return tab;
        }

        private ActionBar.Tab AddTab<TFragment>(string title) where TFragment : Fragment, new()
        {
            return AddTab(title, (sender, e) =>
            {
                e.FragmentTransaction.Replace(Resource.Id.primaryContainer, new TFragment());
            });
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //初始化
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            //构造界面
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.Main);
            AddTab<CardManagementFragment>("校园卡");
            AddTab<AboutFragment>("关于");
            GlobalServices.XjtuSite.Account.IsLoggedInChanged += Account_IsLoggedInChanged;
            await UpdateAccountFragment();
        }

        private async void Account_IsLoggedInChanged(object sender, EventArgs e)
        {
            await UpdateAccountFragment();
        }

        protected override void OnStop()
        {
            base.OnStop();
            GlobalServices.SaveState();
            GlobalServices.XjtuSite.Account.IsLoggedInChanged -= Account_IsLoggedInChanged;
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            DroidUtility.ReportException(this, e.Exception, true);
            e.Handled = true;
        }

        public async Task UpdateAccountFragment()
        {
            if (GlobalServices.XjtuSite.Account.IsInvalidated)
            {
                DroidUtility.ShowToast(this, "正在载入账户信息……");
                await GlobalServices.XjtuSite.Account.UpdateAsync();
            }
            if (GlobalServices.XjtuSite.Account.IsLoggedIn)
	        {
	            if (!(FragmentManager.FindFragmentById(Resource.Id.accountContainer) is AccountProfileFragment))
	            {
	                FragmentManager.BeginTransaction()
	                    .Replace(Resource.Id.accountContainer, new AccountProfileFragment())
	                    .Commit();
	            }
	        }
	        else
	        {
                if (!(FragmentManager.FindFragmentById(Resource.Id.accountContainer) is LoginFragment))
                {
                    FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.accountContainer, new LoginFragment())
                        .Commit();
                }
            }
	    }
	}
}


