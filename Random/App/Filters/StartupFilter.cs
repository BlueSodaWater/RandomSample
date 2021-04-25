using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Random.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random
{
    [SkipScan]
    public class StartupFilter : IStartupFilter
    {
        /// <summary>
        /// dotnet框架响应报文头
        /// </summary>
        private const string DotNetFrameworkResponseHeader = "dotnet-framework";

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                var applicationServices = app.ApplicationServices;

                // 设置应用服务提供器
                App.ApplicationServices = applicationServices;
            };
        }
    }
}
