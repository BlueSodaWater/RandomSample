﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Random.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// 注入启动
[assembly: HostingStartup(typeof(Random.HostingStartup))]

namespace Random
{
    /// <summary>
    /// 配置程序启动时自动注入
    /// </summary>
    [SkipScan]
    public sealed class HostingStartup : IHostingStartup
    {
        /// <summary>
        /// 配置应用启动
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(IWebHostBuilder builder)
        {
            // 自动装载配置
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                AutoAddJsonFile(configurationBuilder);
                AutoAddXmlFile(configurationBuilder);

                // 存储配置
                InternalApp.ConfigurationBuilder = configurationBuilder;
            });

            // 自动注入AppApp()服务
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IStartupFilter, StartupFilter>();

                InternalApp.InternalServices = services;

                services.AddApp();
            });
        }

        /// <summary>
        /// 自动加载自定义.json配置文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        private static void AutoAddJsonFile(IConfigurationBuilder configurationBuilder)
        {
            // 获取程序目录下的所有配置文件
            var jsonNames = Directory.GetFiles(AppContext.BaseDirectory, "*.json", SearchOption.TopDirectoryOnly)
                .Union(
                Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json", SearchOption.TopDirectoryOnly)
                ).Where(u => !excludeJsons.Contains(Path.GetFileName(u)) && !runtimeJsonSuffixs.Any(j => u.EndsWith(j)));

            if (!jsonNames.Any()) return;

            // 自动加载配置文件
            foreach (var jsonName in jsonNames)
            {
                configurationBuilder.AddJsonFile(jsonName, optional: true, reloadOnChange: true);
            }
        }

        /// <summary>
        /// 自动加载自定义 .xml 配置文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        private static void AutoAddXmlFile(IConfigurationBuilder configurationBuilder)
        {
            // 获取程序目录下的所有配置文件，必须以 .config.xml 结尾
            var xmlNames = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                .Union(
                    Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml", SearchOption.TopDirectoryOnly)
                )
                .Where(u => u.EndsWith(".config.xml", StringComparison.OrdinalIgnoreCase));

            if (!xmlNames.Any()) return;

            // 自动加载配置文件
            foreach (var xmlName in xmlNames)
            {
                configurationBuilder.AddXmlFile(xmlName, optional: true, reloadOnChange: true);
            }
        }

        /// <summary>
        /// 默认排除配置项
        /// </summary>
        private static readonly string[] excludeJsons = new[] {
            "appsettings.json",
            "appsettings.Development.json",
            "appsettings.Production.json",
        };

        /// <summary>
        /// 运行时 Json 后缀
        /// </summary>
        private static readonly string[] runtimeJsonSuffixs = new[]
        {
            "deps.json",
            "runtimeconfig.dev.json",
            "runtimeconfig.prod.json",
            "runtimeconfig.json"
        };
    }
}
