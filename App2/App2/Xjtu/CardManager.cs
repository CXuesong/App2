using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.IO;

namespace App2.Xjtu
{
    /// <summary>
    /// card.xjtu.edu.cn
    /// </summary>
    public class CardManager : XjtuSubManager
    {
        public const string SiteRootUrl = "http://card.xjtu.edu.cn";
        public const string CardTransferPageUrl = "http://card.xjtu.edu.cn/CardManage/CardInfo/Transfer";
        public const string CardTransferPostUrl = "http://card.xjtu.edu.cn/CardManage/CardInfo/TransferAccount";
        public const string CardTransferCaptchaKeypadUrl = "http://card.xjtu.edu.cn/Account/GetNumKeyPadImg";

        private decimal? _Balance;

        /// <summary>
        /// 卡面余额。
        /// </summary>
        public decimal? Balance => _Balance;

        private static readonly Regex decimalMatcher = new Regex(@"\d*\.?\d+");

        public async override Task UpdateCoreAsync()
        {
            await base.UpdateCoreAsync();
            Site.GetService<AccountManager>().EnsureLoggedIn();
            var doc = await XjtuUtility.OpenHtmlPageAsync(CardTransferPageUrl, Client);
            // 如果刚刚登录，则会跳转到校园卡管理主页面。
            if (XjtuUtility.GetTitle(doc).Contains("校园卡电子服务平台"))
            {
                doc = await XjtuUtility.OpenHtmlPageAsync(CardTransferPageUrl, Client);
                if (XjtuUtility.GetTitle(doc).Contains("校园卡电子服务平台"))
                    throw new UnexpectedHtmlException(CardTransferPageUrl);
            }
            var str = doc.DocumentNode.Descendants("p")
                .Select(e => e.InnerText)
                .FirstOrDefault(t => t?.Contains("校园卡余额") ?? false);
            var match = decimalMatcher.Match(str);
            if (match.Success)
                _Balance = Convert.ToDecimal(match.Value);
            else
                _Balance = null;
        }

        private static MapAreaInfo[] mapAreas =
        {
            new MapAreaInfo(4, 3, 29, 28, "0"),
            new MapAreaInfo(33, 3, 58, 28, "1"),
            new MapAreaInfo(62, 3, 88, 28, "2"),
            new MapAreaInfo(92, 3, 118, 28, "3"),
            new MapAreaInfo(122, 3, 148, 28, "4"),
            new MapAreaInfo(152, 3, 178, 28, "5"),
            new MapAreaInfo(182, 3, 207, 28, "6"),
            new MapAreaInfo(211, 3, 236, 28, "7"),
            new MapAreaInfo(241, 3, 266, 28, "8"),
            new MapAreaInfo(270, 3, 295, 28, "9"),
            new MapAreaInfo(5, 33, 74, 148, "Backspace"),
            new MapAreaInfo(78, 33, 221, 148, "Clear"),
            new MapAreaInfo(227, 33, 295, 148, "Close"),
        };

        /// <summary>
        /// 向校园卡转账。
        /// </summary>
        public async Task<bool> Transfer(decimal amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            var vcr = Site.RequireService<IVerificationCodeRecognizer>();
            var pp = Site.RequireService<IXjtuCardPasswordProvider>();
            var doc = await XjtuUtility.OpenHtmlPageAsync(CardTransferPageUrl, Client);
            var captchaSrc = doc.GetElementById("img_transCheckCode")?.GetAttributeValue("src", "");
            if (string.IsNullOrEmpty(captchaSrc)) throw new UnexpectedHtmlException(CardTransferPageUrl);
            captchaSrc = SiteRootUrl + captchaSrc;
/*
password=xxxx&checkCode=1222&amt=10.00&fcard=bcard&tocard=card&bankno=&bankpwd=
*/
            var dict = new Dictionary<string, string>()
            {
                {"password", "xxxx"},
                {"checkCode", "xxxx"},
                {"amt", amount.ToString()},
                {"fcard", "bcard"},
                {"tocard", "card"},
                {"bankno", ""},
                {"bankpwd", ""}
            };
            using (var kps = await Client.DownloadStreamAsync(CardTransferCaptchaKeypadUrl))
            {
                var result = await pp.GetPasswordAsync(kps, mapAreas, Site);
                if (result == null) return false;
                dict["password"] = result;
            }
            using (var cs = await Client.DownloadStreamAsync(captchaSrc))
            {
                var result = await vcr.RecognizeAsync(cs, Site);
                if (result == null) return false;
                dict["checkCode"] = result;
            }
            using (var resp = await Client.UploadAsync(CardTransferPostUrl, dict))
            {
                var result = await resp.Content.ReadAsStringAsync();
                var jres = JObject.Parse(result);
                //{"ret":false,"msg":"验证码不正确"}
                if ((bool?)jres["ret"] != true) throw new OperationFailedException((string) jres["msg"]);
            }
            return true;
        }

        internal CardManager(XjtuSiteManager site) : base(site)
        {
            this.Invalidate();
        }
    }
}
