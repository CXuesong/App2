using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2.Droid
{
    static class DroidUtility
    {
        public static void ReportException(Context context, Exception ex, bool details = false)
        {
            using (var t = Toast.MakeText(context, details ? ex.ToString() : ex.Message, ToastLength.Long))
            {
                t.Show();
            }
        }

        public static void ShowToast(Context context, string str)
        {
            using (var t = Toast.MakeText(context, str, ToastLength.Long))
            {
                t.Show();
            }
        }
    }
}