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
        private static WeakReference<Context> _currentContext = new WeakReference<Context>(null);

        /// <summary>
        /// 应用程序当前活动的上下文。（一般为 Activity）
        /// </summary>
        public static Context CurrentContext
        {
            get
            {
                Context c;
                if (_currentContext.TryGetTarget(out c)) return c;
                return null;
            }
            set
            {
                _currentContext.SetTarget(value);
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
                binFormatter.Serialize(fs, GlobalServices.XjtuSite.WebClient.CookieContainer);
            }
        }

        static partial void Initialize()
        {
            // 允许对未处理的异常进行报告。
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private static void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            if (CurrentContext != null)
            {
                DroidUtility.ReportException(CurrentContext, e.Exception, true);
                e.Handled = true;
            }
        }
    }
}