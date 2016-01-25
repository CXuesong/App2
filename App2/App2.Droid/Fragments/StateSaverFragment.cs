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

namespace App2.Droid.Fragments
{
    public class StateSaverFragment : Fragment
    {

        public StateSaverFragment()
        {
            
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
            // Create your fragment here
        }

        public IDictionary<string, object> State { get; } = new Dictionary<string, object>();

        public T TryGetState<T>(string name, T defaultValue = default(T))
        {
            object obj;
            if (State.TryGetValue(name, out obj)) return (T) obj;
            return defaultValue;
        }

        public object TryGetState(string name, object defaultValue = null)
            => TryGetState<object>(name, defaultValue);

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             return inflater.Inflate(Resource.Layout.Empty, container, false);
        }
    }
}