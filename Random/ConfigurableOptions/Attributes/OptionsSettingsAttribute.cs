﻿using Random.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random.ConfigurableOptions
{
    /// <summary>
    /// 选项配置特性
    /// </summary>
    [SkipScan, AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionsSettingsAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OptionsSettingsAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jsonKey">appsetting.json 对应键</param>
        public OptionsSettingsAttribute(string jsonKey)
        {
            JsonKey = jsonKey;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="postConfigureAll">启动所有实例进行后期配置</param>
        public OptionsSettingsAttribute(bool postConfigureAll)
        {
            PostConfigureAll = postConfigureAll;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jsonKey">appsetting.json 对应键</param>
        /// <param name="postConfigureAll">启动所有实例进行后期配置</param>
        public OptionsSettingsAttribute(string jsonKey, bool postConfigureAll)
        {
            JsonKey = jsonKey;
            PostConfigureAll = postConfigureAll;
        }

        /// <summary>
        /// 对应配置文件中的Key
        /// </summary>
        public string JsonKey { get; set; }

        /// <summary>
        /// 对所有配置实例进行后期配置
        /// </summary>
        public bool PostConfigureAll { get; set; }
    }
}
