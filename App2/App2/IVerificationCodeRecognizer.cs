using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    /// <summary>
    /// 封装了用于验证码识别的基本操作。
    /// </summary>
    public interface IVerificationCodeRecognizer
    {
        /// <summary>
        /// 根据指定流中包含的图片，识别验证码。
        /// </summary>
        /// <returns>识别的验证码，或<c>null</c>表示取消。</returns>
        Task<string> RecognizeAsync(Stream imageStream, SiteManager site);
    }

    /*
<map name="keyboardMapForPwd" id="keyboardMapForPwd">
    <area shape="rect" coords="4,3,29,28" value="0" />
    <area shape="rect" coords="33,3,58,28" value="1" />
    <area shape="rect" coords="62,3,88,28" value="2" />
    <area shape="rect" coords="92,3,118,28" value="3" />
    <area shape="rect" coords="122,3,148,28" value="4" />
    <area shape="rect" coords="152,3,178,28" value="5" />
    <area shape="rect" coords="182,3,207,28" value="6" />
    <area shape="rect" coords="211,3,236,28" value="7" />
    <area shape="rect" coords="241,3,266,28" value="8" />
    <area shape="rect" coords="270,3,295,28" value="9" />
    <area shape="rect" coords="5,33,74,148" value="Backspace" />
    <area shape="rect" coords="78,33,221,148" value="Clear" />
    <area shape="rect" coords="227,33,295,148" value="Close" />
</map>
    */
    public interface IXjtuCardPasswordProvider
    {
        Task<string> GetPasswordAsync(Stream keypadImageStream, IList<MapAreaInfo> mapAreas, SiteManager site);
    }

    public struct MapAreaInfo
    {
        public MapAreaInfo(int x1, int y1, int x2, int y2, string value)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Value = value;
        }

        public int X1 { get; }

        public int X2 { get; }

        public int Y1 { get; }

        public int Y2 { get; }

        public int Width => X2 - X1;

        public int Height => Y2 - Y1;

        public string Value { get; }
    }
}
