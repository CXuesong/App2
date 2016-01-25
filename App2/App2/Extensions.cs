using System;
using System.Threading.Tasks;

namespace App2
{
	public static class Extensions
    {
		public static T GetService<T>(this IServiceProvider sp)
		{
		    if (sp == null) throw new ArgumentNullException(nameof(sp));
		    var service = (T) sp.GetService(typeof (T));
		    return service;
		}

	    public static T RequireService<T>(this IServiceProvider sp)
	    {
	        var inst = GetService<T>(sp);
	        if (inst == null) throw new NotSupportedException($"程序需要一个类型为{typeof (T)}的服务，但在当前容器中不存在。");
            return inst;
	    }

	    /// <summary>
        /// 同步执行更新操作。
        /// </summary>
        public static void Update(this IUpdatable updatable)
	    {
	        if (updatable == null) throw new ArgumentNullException(nameof(updatable));
	        Task.WaitAll(updatable.UpdateAsync());
	    }
	}
}

