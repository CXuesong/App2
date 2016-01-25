using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App2.Droid;

namespace App2
{
    partial class GlobalServices
    {
        static partial void LoadXjtuSite()
        {
            try
            {
                using (var fs = Application.Context.OpenFileInput("siteCookies"))
                {
                    var cc = (CookieContainer)binFormatter.Deserialize(fs);
                    XjtuSite.WebClient.CookieContainer = cc;
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception ex)
            {
                DroidUtility.ReportException(Application.Context, ex);
            }
        }

        static partial void SaveXjtuSite()
        {
            using (var fs = Application.Context.OpenFileOutput("siteCookies", FileCreationMode.Private))
            {
                binFormatter.Serialize(fs, GlobalServices.XjtuSite.WebClient.CookieContainer);
            }
        }

        static partial void Initialize()
        {
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private static void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            DroidUtility.ReportException(Application.Context, e.Exception, true);
            e.Handled = true;
        }
    }
}