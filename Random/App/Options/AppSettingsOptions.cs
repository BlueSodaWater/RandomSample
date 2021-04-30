using Microsoft.Extensions.Configuration;
using Random.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random
{
    /// <summary>
    /// 应用全局配置
    /// </summary>
    public sealed class AppSettingsOptions : IConfigurableOptions<AppSettingsOptions>
    {
        /// <summary>
        /// 是否启用引用程序集扫描
        /// </summary>
        public bool? EnabledReferenceAssemblyScan { get; set; }

        /// <summary>
        /// 外部程序集
        /// </summary>
        public string[] ExternalAssemblies { get; set; }

        /// <summary>
        /// 后期配置
        /// </summary>
        /// <param name="options"></param>
        public void PostConfigure(AppSettingsOptions options, IConfiguration configuration)
        {
            EnabledReferenceAssemblyScan ??= false;
            ExternalAssemblies ??= Array.Empty<string>();
        }
    }
}
