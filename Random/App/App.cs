using Microsoft.Extensions.Configuration;
using Random.DependencyInjection;
using System;
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
    }
}
