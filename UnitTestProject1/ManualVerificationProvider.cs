using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2;

namespace UnitTestProject1
{
    class ManualVerificationProvider : IXjtuCardPasswordProvider, IVerificationCodeRecognizer
    {
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public async Task<string> GetPasswordAsync(Stream keypadImageStream, IList<MapAreaInfo> mapAreas, SiteManager site)
        {
            using (var wnd = new XjtuCardCaptchaDialog())
                return wnd.Run(keypadImageStream, site);
        }

        public async Task<string> RecognizeAsync(Stream imageStream, SiteManager site)
        {
            using (var wnd = new XjtuCardCaptchaDialog())
                return wnd.Run(imageStream, site);
        }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
    }
}
