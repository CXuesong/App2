using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Xjtu
{
    public class XjtuSubManager : IUpdatable
    {
        private bool _IsInvalidated = false;

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
            _IsInvalidated = false;
            OnUpdated();
        }

        public virtual async Task UpdateCoreAsync()
        {
            await Task.Yield();
        }

        public event EventHandler Updated;

        public bool IsInvalidated => _IsInvalidated;

        /// <summary>
        /// 设置标记，指示当前对象的内容需要更新了。
        /// </summary>
        public void Invalidate()
        {
            _IsInvalidated = true;
        }

        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}
