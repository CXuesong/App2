using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        string Recognize(Stream imageStream, SiteManager site);
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
        string GetPassword(Stream keypadImageStream, SiteManager site);
    }
}
