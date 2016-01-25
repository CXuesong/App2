using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    /// <summary>
    /// 表示此类型中的内容是可以刷新（注意不是更新）的。
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// 执行刷新操作。
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync();

        event EventHandler Updated;

        /// <summary>
        /// 指示当前对象的内容是否应当更新了。
        /// </summary>
        bool IsInvalidated { get; }
    }
}
