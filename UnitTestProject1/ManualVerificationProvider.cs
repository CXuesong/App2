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
        public string GetPassword(Stream keypadImageStream, SiteManager site)
        {
            using (var wnd = new XjtuCardCaptchaDialog())
                return wnd.Run(keypadImageStream, site);
        }

        public string Recognize(Stream imageStream, SiteManager site)
        {
            using (var wnd = new XjtuCardCaptchaDialog())
                return wnd.Run(imageStream, site);
        }
    }
}
