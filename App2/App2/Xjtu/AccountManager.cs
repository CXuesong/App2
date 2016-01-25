using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;

namespace App2.Xjtu
{
    public class AccountManager : XjtuSubManager
    {
        public const string PortalPageUrl = "http://my.xjtu.edu.cn/";
        public const string LogoutPageUrl = "https://cas.xjtu.edu.cn/logout";

        private bool _IsLoggedIn;
        private string _UserId;
        private string _UserName;

        public event EventHandler IsLoggedInChanged;

        internal AccountManager(XjtuSiteManager site) : base(site)
        {
            this.Invalidate();
        }

        public bool IsLoggedIn => _IsLoggedIn;

        public string UserId => _UserId;

        public string UserName => _UserName;

        /// <summary>
        /// 如果用户未登录，则引发异常。
        /// </summary>
        public void EnsureLoggedIn()
        {
            if (!_IsLoggedIn) throw new InvalidOperationException("用户尚未登录。");
        }

        private Dictionary<string, string> casAuthenticationDictBuffer;
        private string casAuthenticationUrl;

        public override async Task UpdateCoreAsync()
        {
            await base.UpdateCoreAsync();
            var doc = await XjtuUtility.OpenHtmlPageAsync(PortalPageUrl, Client);
            var loginForm = doc.GetElementById("fm1");
            if (loginForm == null)
            {
                //已经登录。
                // my.xjtu.edu.cn
                _IsLoggedIn = true;
                casAuthenticationDictBuffer = null;
                var node = doc.GetElementById("portalWelcome");
                _UserName = HtmlEntity.DeEntitize(node.Element("span").InnerText)?.Trim();
            }
            else
            {
                _IsLoggedIn = false;
                casAuthenticationUrl = "https://cas.xjtu.edu.cn" + loginForm.GetAttributeValue("action", "");
                // 修正 HAP 识别 loginForm 的时候出现的某些问题。  
                //while (loginForm != null && loginForm.ChildNodes.All(n => n.NodeType != HtmlNodeType.Element))
                //{
                //    loginForm = loginForm.NextSibling;
                //}
                //if (loginForm == null) throw new UnexpectedHtmlException(PortalPageUrl);
                loginForm = loginForm.ParentNode;
                if (casAuthenticationDictBuffer == null)
                    casAuthenticationDictBuffer = new Dictionary<string, string>();
                else
                    casAuthenticationDictBuffer.Clear();
                foreach (var e in loginForm.Descendants("input"))
                {
                    var name = e.GetAttributeValue("name", "");
                    if (!string.IsNullOrEmpty(name))
                        casAuthenticationDictBuffer.Add(name, e.GetAttributeValue("value", ""));
                }
                /*
                    TODO: 检查验证码。
<div id="codeDiv" style="float:left;margin:5px 0;display:none;">
<label for="password" style="float:left; line-height:22px;">
验证码&nbsp;
</label>
<input style="float:left;width:60px; border:1px solid #8e8e8e;" type="text" id="code" name="code" maxlength="4">&nbsp;<img style="float:left;" alt="" id="ImageCodeServlet" src="/ImageCodeServlet"><a class="code-text" href="#" onclick="javascript:reloadCode();" title="点击更新验证码">看不清？</a>
</div>    
                */
            }
        }

        private async Task LoginAttemptAsync(string userName, string password)
        {
            /*
username:.....
password:.....
code:
lt:LT-.....
execution:e3s1
_eventId:submit
submit:登录
*/
            if (casAuthenticationDictBuffer == null) await UpdateCoreAsync();
            var dict = casAuthenticationDictBuffer;
            casAuthenticationDictBuffer = null;
            Debug.Assert(dict != null);
            dict["username"] = userName;
            dict["password"] = password;
            var doc = new HtmlDocument();
            using (var resp = await Client.UploadAsync(casAuthenticationUrl, dict))
                doc.LoadHtml(await resp.Content.ReadAsStringAsync());
            var node = doc.GetElementById("msg");
            if (node != null) throw new OperationFailedException(HtmlEntity.DeEntitize(node.InnerText));
            await UpdateCoreAsync();
        }

        public async Task LoginAsync(string userName, string password)
        {
            if (_IsLoggedIn) throw new InvalidOperationException("用户已经登录。");
            await LoginAttemptAsync(userName, password);
            if (!_IsLoggedIn)
            {
                await LoginAttemptAsync(userName, password);
                if (!_IsLoggedIn)
                    throw new UnexpectedDataException("登录流程已经完成，但无法确认登录。这可能是由于网页的结构发生了变动。");
            }
            OnIsLoggedInChanged();
        }

        public async Task LogoutAsync()
        {
            var resp = await Client.TouchAsync(LogoutPageUrl);
            resp.EnsureSuccessStatusCode();
            _UserName = null;
            _UserId = null;
            if (_IsLoggedIn)
            {
                _IsLoggedIn = false;
                OnIsLoggedInChanged();
            }
        }

        protected virtual void OnIsLoggedInChanged()
        {
            IsLoggedInChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}