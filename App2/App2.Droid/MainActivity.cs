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
	[Activity (Label = "App2.Droid", Icon = "@drawable/icon")]
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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //初始化
            if (!GlobalServices.XjtuSite.Account.IsLoggedIn)
            {
                RequestLogin();
                return;
            }
            GlobalServices.XjtuSite.Account.IsLoggedInChanged += Account_IsLoggedInChanged;
            //构造界面
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.Main);
            AddTab<CardManagementFragment>("校园卡");
            AddTab<AboutFragment>("关于");
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.accountContainer, new AccountProfileFragment())
                .Commit();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GlobalServices.XjtuSite.Account.IsLoggedInChanged -= Account_IsLoggedInChanged;
        }

        private void Account_IsLoggedInChanged(object sender, EventArgs e)
        {
            if (!GlobalServices.XjtuSite.Account.IsLoggedIn) RequestLogin();
        }

	    private void RequestLogin()
	    {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

	    protected override void OnStop()
        {
            base.OnStop();
            GlobalServices.SaveState();
        }
	}
}


