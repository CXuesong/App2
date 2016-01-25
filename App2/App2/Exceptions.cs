using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace App2
{
    /// <summary>
    /// 表示因接受到的数据与期望不符而引发的异常。
    /// </summary>
    public class UnexpectedDataException : InvalidOperationException
    {
        public UnexpectedDataException()
            : this("接收到了预料之外的数据。")
        { }

        public UnexpectedDataException(string message)
            : base(message)
        { }

        public UnexpectedDataException(string message, Exception inner)
            : base(message, inner)
        { }
#if !WINDOWS_PHONE_APP
        public UnexpectedDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
#endif
    }

    public class UnexpectedHtmlException : UnexpectedDataException
    {
        public UnexpectedHtmlException()
            : this("无法解析接收到的页面。")
        { }

        public UnexpectedHtmlException(string url)
            : base($"无法解析接收到的页面：{url}。")
        { }

        public UnexpectedHtmlException(string url, string message)
            : base($"无法解析接收到的页面：{url}。{message}")
        { }

        public UnexpectedHtmlException(Uri uri)
    : base($"无法解析接收到的页面：{uri}。")
        { }

        public UnexpectedHtmlException(string message, Exception inner)
            : base(message, inner)
        { }
#if !WINDOWS_PHONE_APP
        public UnexpectedHtmlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
#endif
    }

    /// <summary>
    /// 表示因操作失败而引发的异常。
    /// </summary>
    public class OperationFailedException : InvalidOperationException
    {
        private readonly int _ErrorCode;
        private readonly string _ErrorMessage;

        /// <summary>
        /// 获取异常代码。
        /// </summary>
        public int ErrorCode => _ErrorCode;

        /// <summary>
        /// 获取异常消息。
        /// </summary>
        public string ErrorMessage => _ErrorMessage;

        public OperationFailedException()
            : this("操作失败。")
        { }

        public OperationFailedException(int errorCode, string errorMessage)
            : base(
                string.Format(
                    string.IsNullOrEmpty(errorMessage)
                        ? "操作失败：{0}。"
                        : "操作失败：{0}。{1}",
                    errorCode, errorMessage))
        {
            _ErrorCode = errorCode;
            _ErrorMessage = errorMessage;
        }

        public OperationFailedException(int errorCode)
            : this(errorCode, null)
        {

        }

        public OperationFailedException(string message)
            : base(message)
        { }

        public OperationFailedException(string message, Exception inner)
            : base(message, inner)
        { }
#if !WINDOWS_PHONE_APP
        public OperationFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _ErrorCode = info.GetInt32("ErrorCode");
            _ErrorMessage = info.GetString("ErrorMessage");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ErrorCode", _ErrorCode);
            info.AddValue("ErrorMessage", _ErrorMessage);
        }
#endif
    }


    /// <summary>
    /// 表示因操作失败而引发的异常。
    /// </summary>
    public class HttpOperationFailedException : OperationFailedException
    {
        public HttpOperationFailedException(int statusCode)
            : base(statusCode, "Http错误，请参阅错误代码以了解详情。")
        { }
    }
}
