using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2.Xjtu;
using App2;

namespace UnitTestProject1
{
    static partial class Utility
    {
        /// <summary>
        /// 在此处完成登录操作。
        /// </summary>
        static partial void AuthenticateCore(AccountManager account);

        public static XjtuSiteManager CreateXjtuSiteManager()
        {
            var newInst = new XjtuSiteManager();
            newInst.RegisterService(new ManualVerificationProvider());
            return newInst;
        }

        public static void LoginSiteManager(XjtuSiteManager m)
        {
            if (m == null) throw new ArgumentNullException(nameof(m));
            var a = m.RequireService<AccountManager>();
            AuthenticateCore(a);
            Trace.Assert(a.IsLoggedIn);
        }

        public static void LogoutSiteManager(XjtuSiteManager m)
        {
            if (m == null) throw new ArgumentNullException(nameof(m));
            var a = m.RequireService<AccountManager>();
            a.LogoutAsync().Wait();
        }

    }


}
