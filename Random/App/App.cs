using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Random.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
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

        /// <summary>
        /// 应用有效程序集
        /// </summary>
        public static readonly IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 能够被扫描的类型
        /// </summary>
        public static readonly IEnumerable<Type> CanBeScanTypes;

        /// <summary>
        /// 构造函数
        /// </summary>
        static App()
        {
            Configuration = InternalApp.ConfigurationBuilder.Build();

            Assemblies = GetAssemblies();
            CanBeScanTypes = Assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsPublic && !u.IsDefined(typeof(SkipScanAttribute), false)));

            Startups = new ConcurrentBag<AppStartup>();
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="jsonKey">配置中对应的Key</param>
        /// <returns>TOptions</returns>
        public static TOptions GetOptions<TOptions>(string jsonKey)
            where TOptions : class, new()
        {
            return Configuration.GetSection(jsonKey).Get<TOptions>();
        }

        /// <summary>
        /// 获取应用有效程序集
        /// </summary>
        /// <returns>IEnumerable<Assembly></returns>
        internal static IEnumerable<Assembly> GetAssemblies()
        {
            // 需要排除的程序集后缀
            var exculdeAssemblyNames = new string[]
            {
                "Database.Migrations"
            };

            // 读取应用配置
            var settings = GetOptions<AppSettingsOptions>("AppSettings") ?? new AppSettingsOptions { };

            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或发布的包，或手动添加引用的dll
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(u => (u.Type == "project" && !exculdeAssemblyNames.Any(j => u.Name.EndsWith(j)))
                    || (u.Type == "package" && u.Name.StartsWith(nameof(Random)))
                    || (settings.EnabledReferenceAssemblyScan == true && u.Type == "reference"))
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
                .ToList();

            // 加载 'appsetting.json' 配置的外部程序集
            if (settings.ExternalAssemblies != null && settings.ExternalAssemblies.Any())
            {
                foreach (var externalAssembly in settings.ExternalAssemblies)
                {
                    scanAssemblies.Add(Assembly.Load(externalAssembly));
                }
            }

            return scanAssemblies;
        }
    }

    /// <summary>
    /// 内部App副本
    /// </summary>
    internal static class InternalApp
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection InternalServices;

        /// <summary>
        /// 全局配置构建器
        /// </summary>
        internal static IConfigurationBuilder ConfigurationBuilder;
    }
}
