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
        private static readonly WeakReference<Activity> _currentActivity = new WeakReference<Activity>(null);

        /// <summary>
        /// 应用程序当前活动的 Activity 。
        /// </summary>
        public static Activity CurrentActivity
        {
            get
            {
                Activity c;
                if (_currentActivity.TryGetTarget(out c)) return c;
                return null;
            }
            set
            {
                _currentActivity.SetTarget(value);
            }
        }

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
                binFormatter.Serialize(fs, XjtuSite.WebClient.CookieContainer);
            }
        }

        static partial void Initialize()
        {
            // 允许对未处理的异常进行报告。
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private static void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            if (CurrentActivity != null)
            {
                DroidUtility.ReportException(CurrentActivity, e.Exception, true);
                e.Handled = true;
            }
        }
    }
}