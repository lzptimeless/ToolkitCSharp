using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Configs
{
    /// <summary>
    /// 配置文件接口
    /// Config interface
    /// </summary>
    public interface IConfig : IDisposable
    {
        /// <summary>
        /// 配置文件路径-Log file path
        /// </summary>
        string Path { get; }
        /// <summary>
        /// 配置文件是否已经加载-Log file is loaded from file
        /// </summary>
        bool IsLoaded { get; }
        /// <summary>
        /// 加载配置文件-Load config file
        /// </summary>
        /// <returns></returns>
        Task LoadAsync();
        /// <summary>
        /// 保存到本地-Save to local file
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
