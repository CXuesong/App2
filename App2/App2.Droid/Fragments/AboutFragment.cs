using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App2.Droid.Fragments
{
    public class AboutFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.About, container, false);
            var versionTextView = view.FindViewById<TextView>(Resource.Id.versionTextView);
            var pm = Activity.PackageManager.GetPackageInfo(Activity.PackageName, default(PackageInfoFlags));
            versionTextView.Text = $"{pm.VersionName} (v{pm.VersionCode})";
            return view;
        }
    }
}