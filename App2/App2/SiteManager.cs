using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using App2.Xjtu;

namespace App2
{
    public abstract class SiteManager : IServiceProvider, IDisposable
    {
        private PortableWebClient _WebClient = new PortableWebClient();
        private List<object> services = new List<object>();
        private bool _IsDisposed;

        public virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof (PortableWebClient)) return _WebClient;
            var ti = serviceType.GetTypeInfo();
            return services.FirstOrDefault(s => ti.IsAssignableFrom(s.GetType().GetTypeInfo()));
        }

        public void RegisterService(object service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            services.Add(service);
        }

        public bool UnregisterService(object service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return services.Remove(service);
        }

        #region IDisposable Support

        public bool IsDisposed => _IsDisposed;

        public PortableWebClient WebClient => _WebClient;

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _WebClient.Dispose();
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                _IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~XjtuPortalManager() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。

        #endregion
    }
}
