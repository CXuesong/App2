using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace App2.Xjtu
{
    internal static class XjtuUtility
    {
        public static async Task<HtmlDocument> OpenHtmlPageAsync(string url, PortableWebClient client)
        {
            var doc = new HtmlDocument();
            LOAD_PAGE:
            doc.LoadHtml(await client.DownloadStringAsync(url));
            string title = GetTitle(doc);
            // 处理重定向。
            if (title.ToLowerInvariant().Contains("object moved"))
            {
                var nextUrl = doc.DocumentNode.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(nextUrl))
                {
                    url = nextUrl;
                    goto LOAD_PAGE;
                }
                throw new UnexpectedHtmlException(url, "无法解析重定向目标。");
            }
            // 有时 CAS 中间会多一步。
            if (title.Contains("统一身份认证网关"))
            {
                var loginForm = doc.GetElementById("fm1");
                if (loginForm?.Name != "form") loginForm = null;
                if (loginForm == null)
                {
                    /*
<div id="content" class="fl-screenNavigator-scroll-container"  >
<div class="info"><p>单击 <a href="http://my.xjtu.edu.cn/Login?ticket=....">这里</a> ，便能够访问到目标应用。</p></div>
</div>
                    */
                    var node = doc.GetElementById("content");
                    var nextUrl = node?.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(nextUrl))
                    {
                        url = nextUrl;
                        goto LOAD_PAGE;
                    }
                    throw new UnexpectedHtmlException(url, "无法解析重定向目标。");
                }
            }
            return doc;
        }

        public static string GetTitle(HtmlDocument doc)
        {
            return doc.DocumentNode.Element("html")
                ?.Element("head")
                ?.Element("title")
                ?.InnerText ?? "";
        }
    }
}
