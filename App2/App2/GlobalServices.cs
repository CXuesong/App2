using System;
using System.Collections.Generic;
using System.Net;
#if !WINDOWS_PHONE_APP
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.Text;
using App2.Xjtu;

namespace App2
{
    /// <summary>
    /// 为应用程序提供一些全局状态。
    /// </summary>
    internal static partial class GlobalServices
    {
        private static XjtuSiteManager _XjtuSite;
#if !WINDOWS_PHONE_APP
        private static readonly BinaryFormatter binFormatter = new BinaryFormatter();
#endif

        public static XjtuSiteManager XjtuSite
        {
            get
            {
                if (_XjtuSite == null)
                {
                    _XjtuSite = new XjtuSiteManager();
                    LoadXjtuSite();
                }
                return _XjtuSite;
            }
        }

        public static void SaveState()
        {
            SaveXjtuSite();
        }

        static partial void SaveXjtuSite();

        static partial void LoadXjtuSite();

        static partial void Initialize();

        static GlobalServices()
        {
            Initialize();
        }

    }
}
