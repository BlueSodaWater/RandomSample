using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Random.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

                // 设置响应报文头信息，标记框架类型
                app.Use(async (context, next) =>
                {
                    context.Response.Headers[DotNetFrameworkResponseHeader] = "Random";
                    await next.Invoke();
                });

                // 调用默认中间件
                app.UseApp();

                UseStartup(app, applicationServices);

                // 调用 Random.Web.Entry 中的 Startup
                next(app);
            };
        }

        /// <summary>
        /// 配置 Startup 的 Configure
        /// </summary>
        /// <param name="app">应用构建器</param>
        /// <param name="applicationServices">服务提供器</param>
        private static void UseStartup(IApplicationBuilder app,IServiceProvider applicationServices)
        {
            var startups = App.Startups;
            if (!startups.Any()) return;

            var env = applicationServices.GetRequiredService<IWebHostEnvironment>();

            // 获取环境和配置
            foreach (var startup in startups)
            {
                var type = startup.GetType();

                // 获取所有符合依赖注入格式的方法，如返回值void，且第一个参数是 IApplicationBuilder 类型，第二个参数是 IWebHostEnvironment
                var configureMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(u => u.ReturnType == typeof(void)
                        && u.GetParameters().Length > 1
                        && u.GetParameters()[0].ParameterType == typeof(IApplicationBuilder)
                        && u.GetParameters()[1].ParameterType == typeof(IWebHostEnvironment));

                if (!configureMethods.Any()) continue;

                // 自动安装属性调用
                foreach (var method in configureMethods)
                {
                    method.Invoke(startup, new object[] { app, env });
                }
            }
        }
    }
}
