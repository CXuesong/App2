using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace App2.Xjtu
{
    public class XjtuSiteManager : SiteManager
    {
        private AccountManager account;
        private CardManager card;

        public AccountManager Account => account;

        public CardManager Card => card;

        public override object GetService(Type serviceType)
        {
            if (serviceType == typeof(AccountManager)) return account;
            if (serviceType == typeof(CardManager)) return card;
            return base.GetService(serviceType);
        }

        public XjtuSiteManager()
        {
            account = new AccountManager(this);
            card = new CardManager(this);
        }
    }
}
