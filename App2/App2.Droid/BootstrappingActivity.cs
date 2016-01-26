using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2.Droid
{
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BootstrappingActivity : MyActivity
    {
        private View offlineNotice;
        private Button retryButton;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Bootstrapping);
            //var splashImageView = FindViewById<ImageView>(Resource.Id.splashImageView);
            offlineNotice = FindViewById<View>(Resource.Id.offlineNotice);
            retryButton = FindViewById<Button>(Resource.Id.retryButton);
            retryButton.Click += RetryButton_Click;
            await Redirect();
        }

        private async void RetryButton_Click(object sender, EventArgs e)
        {
            await Redirect();
        }

        private async Task Redirect()
        {
            offlineNotice.Visibility = ViewStates.Gone;
            if (GlobalServices.XjtuSite.Account.IsInvalidated)
            {
                try
                {
                    await GlobalServices.XjtuSite.Account.UpdateAsync();
                }
                catch (Exception ex) when (ex is WebException || ex is TaskCanceledException)
                {
                    offlineNotice.Visibility = ViewStates.Visible;
                    DroidUtility.ReportException(this, ex);
                    return;
                }
            }
            var intent = new Intent(this, GlobalServices.XjtuSite.Account.IsLoggedIn
                ? typeof(MainActivity)
                : typeof(LoginActivity));
            StartActivity(intent);
            this.Finish();
        }
    }
}