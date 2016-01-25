using System;
using System.Diagnostics;
using System.Threading.Tasks;
using App2;
using App2.Xjtu;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class XjtuSiteTests
    {
        [TestMethod]
        public async Task AccountTest()
        {
            using (var m = Utility.CreateXjtuSiteManager())
            {
                var a = m.RequireService<AccountManager>();
                await a.UpdateCoreAsync();
                Trace.WriteLine($"IsLoggedIn = {a.IsLoggedIn}");
                //await a.LoginAsync("abc", "def");
                Utility.LoginSiteManager(m);
                Trace.WriteLine($"IsLoggedIn = {a.IsLoggedIn}");
                Trace.WriteLine($"UserName = {a.UserName}");
                await a.LogoutAsync();
            }
        }

        [TestMethod]
        public async Task CardTransferTest()
        {
            using (var m = Utility.CreateXjtuSiteManager())
            {
                Utility.LoginSiteManager(m);
                var cm = m.RequireService<CardManager>();
                cm.Update();
                Trace.WriteLine($"Balance = {cm.Balance}");
                await cm.Transfer((decimal) 1.0);
                Utility.LogoutSiteManager(m);
            }
        }
    }
}
