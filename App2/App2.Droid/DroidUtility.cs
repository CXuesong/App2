using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App2.Droid
{
    static class DroidUtility
    {
        public static void ReportException(Context context, Exception ex, bool details = false)
        {
#if DEBUG
            details = true;
#endif
            using (var t = Toast.MakeText(context, details ? ex.ToString() : $"{ex.GetType()}: {ex.Message}", ToastLength.Long))
            {
                t.Show();
            }
        }

        public static void ShowToast(Context context, string str)
        {
            using (var t = Toast.MakeText(context, str, ToastLength.Short))
            {
                t.Show();
            }
        }

        private static DisplayMetrics _DisplayMetrics;

        public static DisplayMetrics DisplayMetrics
        {
            get
            {
                if (_DisplayMetrics == null)
                {
                    _DisplayMetrics = new DisplayMetrics();
                    GlobalServices.CurrentActivity.WindowManager.DefaultDisplay.GetMetrics(_DisplayMetrics);
                }
                return _DisplayMetrics;
            }
        }

        public static int DipToPixelsX(float dip)
        {
            return (int) (dip*DisplayMetrics.Xdpi/160.0);
        }

        public static int DipToPixelsY(float dip)
        {
            return (int) (dip*DisplayMetrics.Ydpi/160.0);
        }
    }
}