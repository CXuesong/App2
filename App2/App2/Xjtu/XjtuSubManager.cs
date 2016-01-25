using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Xjtu
{
    public class XjtuSubManager : IUpdatable
    {
        internal XjtuSubManager(XjtuSiteManager site)
        {
            if (site == null) throw new ArgumentNullException(nameof(site));
            Site = site;
            Client = site.GetService<PortableWebClient>();
        }

        protected PortableWebClient Client { get; }

        public XjtuSiteManager Site { get; }

        public async Task UpdateAsync()
        {
            await UpdateCoreAsync();
            OnUpdated();
        }

        public virtual async Task UpdateCoreAsync()
        {
            await Task.Yield();
        }

        public event EventHandler Updated;

        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}
