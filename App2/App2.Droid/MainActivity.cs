using System;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
	public class MainActivity : Activity, IServiceProvider
	{
	    private XjtuSiteManager site;

        private bool stateInvalidated = true;

	    private void EnsureStateLoaded()
	    {
	        if (!stateInvalidated) return;
            var stateSaver = FragmentManager.FindFragmentById<StateSaverFragment>(Resource.Id.stateSaver);
            site = stateSaver.TryGetState<XjtuSiteManager>("site") ?? new XjtuSiteManager();
	        stateInvalidated = false;
	    }

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            SetContentView(Resource.Layout.Main);
            EnsureStateLoaded();
            site.Account.IsLoggedInChanged += (_, e) =>
		    {
		        UpdateAccountFragment();
		    };
		    UpdateAccountFragment();
		}

        protected override void OnStop()
        {
            base.OnStop();
            SaveSiteManager();
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            DroidUtility.ReportException(this, e.Exception, true);
            e.Handled = true;
        }

        public object GetService(Type serviceType)
	    {
            if (serviceType == typeof(XjtuSiteManager))
                return site;
            else
                return site.GetService(serviceType);
	    }

        private static readonly BinaryFormatter seriFormatter = new BinaryFormatter();

	    public void SaveSiteManager()
	    {
	        using (var fs = ApplicationContext.OpenFileOutput("siteCookies", FileCreationMode.Private))
	        {
	            seriFormatter.Serialize(fs, site.WebClient.CookieContainer);
	        }
        }

        public XjtuSiteManager LoadSiteManager()
        {
            var newInst = new XjtuSiteManager();
            try
            {
                using (var fs = ApplicationContext.OpenFileOutput("siteCookies", FileCreationMode.Private))
                {
                    var cc = (CookieContainer) seriFormatter.Deserialize(fs);
                    newInst.WebClient.CookieContainer = cc;
                }
            }
            catch (FileNotFoundException)
            {
            }
            return newInst;
        }

        public void UpdateAccountFragment()
	    {
	        if (site.Account.IsLoggedIn)
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


