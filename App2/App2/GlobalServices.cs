using System;
using System.Collections.Generic;
using System.Text;
using App2.Xjtu;

namespace App2
{
    /// <summary>
    /// 为应用程序提供一些全局状态。
    /// </summary>
    internal static class GlobalServices
    {
        private static XjtuSiteManager _XjtuSite;

        public static XjtuSiteManager XjtuSite
        {
            get
            {
                if (_XjtuSite == null) _XjtuSite = new XjtuSiteManager();
                return _XjtuSite;
            }
        }
    }
}
