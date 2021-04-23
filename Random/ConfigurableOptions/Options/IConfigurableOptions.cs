using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random.ConfigurableOptions
{
    /// <summary>
    /// 应用选项依赖接口
    /// </summary>
    public partial interface IConfigurableOptions { }

    /// <summary>
    /// 选项后期配置
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public partial interface IConfigurableOptions<TOptions> : IConfigurableOptions
        where TOptions : class, IConfigurableOptions
    {
        void PostConfigure(TOptions options, IConfiguration configuration);
    }
}
