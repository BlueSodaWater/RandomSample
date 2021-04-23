using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Web 主机构建器拓展类
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Web 主机注入StartupFilter 和 HostStartup
        /// </summary>
        /// <param name="webBuilder">Web主机构建器</param>
        /// <param name="assemblyName">外部程序集名称</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder Inject(this IWebHostBuilder webBuilder, string assemblyName = nameof(Random))
        {
            webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, assemblyName);
            return webBuilder;
        }
    }
}
