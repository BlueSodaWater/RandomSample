using Microsoft.Extensions.Configuration;
using Random.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random
{
    /// <summary>
    /// 全局应用类
    /// </summary>
    [SkipScan]
    public static class App
    {
        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static readonly IConfiguration Configuration;

        /// <summary>
        /// 应用所有启动配置对象
        /// </summary>
        internal static ConcurrentBag<AppStartup> Startups;

        /// <summary>
        /// 应用服务提供器
        /// </summary>
        public static IServiceProvider ApplicationServices { get; internal set; }
    }
}
