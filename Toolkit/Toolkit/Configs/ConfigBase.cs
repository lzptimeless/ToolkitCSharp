using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Configs
{
    /// <summary>
    /// 配置文件基础类，基本实现了<see cref="IConfig"/>
    /// Config basic class, implement <see cref="IConfig"/>
    /// </summary>
    public abstract class ConfigBase : IConfig
    {
        #region fields
        /// <summary>
        /// 写入日志到本地的IO-The IO use to write log to local file
        /// </summary>
        private StreamWriter _logWriter;
        #endregion

        #region constructors
        /// <summary>
        /// 初始化<see cref="ConfigBase"/>-Initialize <see cref="ConfigBase"/>
        /// </summary>
        /// <param name="path">配置文件路径-Config file path</param>
        public ConfigBase(string path)
        {
            _path = path;
        }
        #endregion

        #region properties
        #region IsLoaded
        private bool _isLoaded;
        public bool IsLoaded
        {
            get { return this._isLoaded; }
        }
        #endregion

        #region Path
        private string _path;
        public string Path
        {
            get { return this._path; }
        }
        #endregion
        #endregion

        #region public methods
        public async Task LoadAsync()
        {
            var stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 4096, true);
            var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true);
            _logWriter = new StreamWriter(stream, Encoding.UTF8, 4096, false);
            _logWriter.AutoFlush = true;

            string content = await reader.ReadToEndAsync();
            Unserialize(content);

            reader.Dispose();
            _isLoaded = true;
        }

        public Task SaveAsync()
        {
            string content = Serialize();
            return _logWriter.WriteAsync(content);
        }

        public void Dispose()
        {
            _logWriter.Dispose();
        }
        #endregion

        #region private methods
        /// <summary>
        /// 需要子类实现的，反序列化文本为配置数据
        /// </summary>
        protected abstract void Unserialize(string content);

        /// <summary>
        /// 需要子类实现的，序列化配置数据为文本
        /// </summary>
        /// <returns></returns>
        protected abstract string Serialize();
        #endregion
    }
}
