using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App2
{
    public class PortableWebClient : IDisposable
    {
        private readonly MyWebClient client = MyWebClient.Create();

        public PortableWebClient()
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.CookieContainer = new CookieContainer();
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            //client.DefaultRequestHeaders.Referrer = new Uri("https://cas.xjtu.edu.cn/login?service=http://my.xjtu.edu.cn/Login");
        }

        public CookieContainer CookieContainer
        {
            get { return client.CookieContainer; }
            set { client.CookieContainer = value; }
        }

        public string UserAgent
        {
            get
            {
                return client.DefaultRequestHeaders.UserAgent.ToString();
            }
            set
            {
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(value);
            }
        }

        public async Task<string> DownloadStringAsync(string address)
        {
            return await client.GetStringAsync(address);
        }

        public async Task<Stream> DownloadStreamAsync(string address)
        {
            return await client.GetStreamAsync(address);
        }

        public async Task<byte[]> DownloadByteArrayAsync(string address)
        {
            return await client.GetByteArrayAsync(address);
        }

        public async Task<HttpResponseMessage> UploadAsync(string address, string content)
        {
            var c = new StringContent(content);
            return await client.PostAsync(address, c);
        }

        public async Task<HttpResponseMessage> UploadAsync(string address, IEnumerable<KeyValuePair<string, string>> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var c = new FormUrlEncodedContent(data);
            return await client.PostAsync(address, c);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            //client.FollowRedirections = followRedirections;
            //try
            //{
                return client.SendAsync(request, completionOption);
            //}
            //finally
            //{
            //    client.FollowRedirections = true;
            //}
        }

        public Task<HttpResponseMessage> TouchAsync(string address)
        {
            return client.SendAsync(new HttpRequestMessage(HttpMethod.Get, address),
                HttpCompletionOption.ResponseHeadersRead);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    client.Dispose();
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~PortableWebClient() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
    internal class MyWebClient : HttpClient
    {
        private HttpClientHandler handler = new HttpClientHandler();

        public CookieContainer CookieContainer
        {
            get
            {
                return handler.CookieContainer;
            }
            set
            {
                if (value == null)
                {
                    handler.UseCookies = false;
                }
                else
                {
                    handler.CookieContainer = value;
                    handler.UseCookies = true;
                }
            }
        }

        public bool FollowRedirections
        {
            get { return handler.AllowAutoRedirect; }
            set { handler.AllowAutoRedirect = value; }
        }

        public static MyWebClient Create()
        {
            var handler = new HttpClientHandler();
            var newInst = new MyWebClient(handler);
            newInst.handler = handler;
            return newInst;
        }

        private MyWebClient(HttpClientHandler handler) : base(handler)
        {
        }
    }

    //#if WINDOWS_PHONE_APP
    //    internal class MyWebClient : HttpClient
    //    {
    //        private HttpClientHandler handler = new HttpClientHandler();

    //        public CookieContainer CookieContainer
    //        {
    //            get
    //            {
    //                return handler.CookieContainer;
    //            }
    //            set
    //            {
    //                if (value == null)
    //                {
    //                    handler.UseCookies = false;
    //                }
    //                else
    //                {
    //                    handler.CookieContainer = value;
    //                }
    //            }
    //        }

    //        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //        {
    //            request.Headers
    //            return base.SendAsync(request, cancellationToken);
    //        }

    //        public static MyWebClient Create()
    //        {
    //            var handler = new HttpClientHandler();
    //            var newInst = new MyWebClient(handler);
    //            newInst.handler = handler;
    //            return newInst;
    //        }

    //        private MyWebClient(HttpClientHandler handler) : base(handler)
    //        {
    //        }
    //    }
    //#else
    //    internal class MyWebClient : WebClient
    //    {
    //        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    //        public CookieContainer CookieContainer { get; set; }

    //        protected override WebRequest GetWebRequest(Uri address)
    //        {
    //            var request = base.GetWebRequest(address);
    //            var webRequest = request as HttpWebRequest;
    //            request.Timeout = (int) Timeout.TotalMilliseconds;
    //            if (webRequest != null)
    //            {
    //                webRequest.CookieContainer = this.CookieContainer;
    //            }
    //            return request;
    //        }

    //        public static MyWebClient Create()
    //        {
    //            var newInst = new MyWebClient();
    //            return newInst;
    //        }
    //    }
    //#endif
}
