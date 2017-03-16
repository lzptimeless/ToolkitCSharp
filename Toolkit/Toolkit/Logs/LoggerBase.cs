using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Logs
{
    /// <summary>
    /// 日志记录器基础类，实现了一些基本功能
    /// Logger base class, implemented some base function
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        #region fields
        /// <summary>
        /// 入口程序集版本号-Entry assembly version
        /// </summary>
        private string _version;
        #endregion

        #region constructors
        public LoggerBase()
        {
            this._version = this.GetVersion();
        }
        #endregion

        #region properties

        #endregion

        #region public methods
        public void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            string log = this.CreateLog("DEBUG", message, callerName, callerFilePath, callerLineNumber);
            this.OnDebug(log);
        }

        public void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            string log = this.CreateLog("ERROR", message, callerName, callerFilePath, callerLineNumber);
            this.OnError(log);
        }

        public void Info(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            string log = this.CreateLog("INFO", message, callerName, callerFilePath, callerLineNumber);
            this.OnInfo(log);
        }

        public void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            string log = this.CreateLog("WARN", message, callerName, callerFilePath, callerLineNumber);
            this.OnWarn(log);
        }
        #endregion

        #region private methods
        /// <summary>
        /// 需要子类实现的将已经整合好的DEBUG类型日志记录的函数
        /// </summary>
        /// <param name="log">已经整合好的日志记录</param>
        protected abstract void OnDebug(string log);

        /// <summary>
        /// 需要子类实现的将已经整合好的INFO类型日志记录的函数
        /// </summary>
        /// <param name="log">已经整合好的日志记录</param>
        protected abstract void OnInfo(string log);

        /// <summary>
        /// 需要子类实现的将已经整合好的WARN类型日志记录的函数
        /// </summary>
        /// <param name="log">已经整合好的日志记录</param>
        protected abstract void OnWarn(string log);

        /// <summary>
        /// 需要子类实现的将已经整合好的ERROR类型日志记录的函数
        /// </summary>
        /// <param name="log">已经整合好的日志记录</param>
        protected abstract void OnError(string log);

        /// <summary>
        /// 将日志信息整合为最终将要写入日志文件的字符串
        /// Combine log information into one <see cref="string"/>
        /// </summary>
        /// <param name="category">日志分类</param>
        /// <param name="message">日志内容</param>
        /// <param name="callerName">当前函数名</param>
        /// <param name="callerFilePath">当前代码所在文件名</param>
        /// <param name="callerLineNumber">当前代码所在行数</param>
        /// <returns></returns>
        protected virtual string CreateLog(string category, string message, string callerName, string callerFilePath, int callerLineNumber)
        {
            string time = this.GetTime();
            string fileName = null;
            if (!string.IsNullOrWhiteSpace(callerFilePath))
            {
                try
                {
                    fileName = Path.GetFileName(callerFilePath);
                }
                catch
                {
                    fileName = callerFilePath;
                }
            }

            string msg = string.Format("{0,-5} TIME:{1}, VERSION:{2}, CALLER:{3}, FILE:{4}, LINE:{5}\r\n{6}\r\n",
                category,
                time,
                this._version,
                callerName,
                fileName,
                callerLineNumber,
                message);

            return msg;
        }

        /// <summary>
        /// 获取入口程序集版本号，格式为xx.xx.xx.xx
        /// Get entry assembly version, format: xx.xx.xx.xx
        /// </summary>
        /// <returns></returns>
        protected virtual string GetVersion()
        {
            // xxx.xxx.xxx.xxx
            string version = Assembly.GetEntryAssembly().GetName().Version.ToString(4);
            return version;
        }

        /// <summary>
        /// 获取当前时间，格式为年-月-日 时:分:秒.毫秒 时区
        /// Get current time, format: year-moth-day hour:minute:second.millisecond timezone
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTime()
        {
            // year-moth-day hour:minute:second.millisecond timezone
            string time = DateTime.Now.ToString("yyyy-MM-dd HH\\:mm\\:ss.fff z");
            return time;
        }
        #endregion
    }
}
