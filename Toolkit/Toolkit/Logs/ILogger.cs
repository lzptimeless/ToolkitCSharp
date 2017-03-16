using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Logs
{
    /// <summary>
    /// 日志记录器接口
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 记录DEBUG分类的日志
        /// Record DEBUG log
        /// </summary>
        /// <param name="message">日志内容-Log content</param>
        /// <param name="callerName">当前函数名，自动生成，无需赋值-Current function name, will be assigned automatically</param>
        /// <param name="callerFilePath">当前代码文件名，自动生成，无需赋值-Current code file name, will be assigned automatically</param>
        /// <param name="callerLineNumber">当前代码行数，自动生成，无需赋值-Current code line number, will be assigned automatically</param>
        void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        /// <summary>
        /// 记录INFO分类的日志
        /// Record INFO log
        /// </summary>
        /// <param name="message">日志内容-Log content</param>
        /// <param name="callerName">当前函数名，自动生成，无需赋值-Current function name, will be assigned automatically</param>
        /// <param name="callerFilePath">当前代码文件名，自动生成，无需赋值-Current code file name, will be assigned automatically</param>
        /// <param name="callerLineNumber">当前代码行数，自动生成，无需赋值-Current code line number, will be assigned automatically</param>
        void Info(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        /// <summary>
        /// 记录WARN分类的日志
        /// Record WARN log
        /// </summary>
        /// <param name="message">日志内容-Log content</param>
        /// <param name="callerName">当前函数名，自动生成，无需赋值-Current function name, will be assigned automatically</param>
        /// <param name="callerFilePath">当前代码文件名，自动生成，无需赋值-Current code file name, will be assigned automatically</param>
        /// <param name="callerLineNumber">当前代码行数，自动生成，无需赋值-Current code line number, will be assigned automatically</param>
        void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        /// <summary>
        /// 记录ERROR分类的日志
        /// Record ERROR log
        /// </summary>
        /// <param name="message">日志内容-Log content</param>
        /// <param name="callerName">当前函数名，自动生成，无需赋值-Current function name, will be assigned automatically</param>
        /// <param name="callerFilePath">当前代码文件名，自动生成，无需赋值-Current code file name, will be assigned automatically</param>
        /// <param name="callerLineNumber">当前代码行数，自动生成，无需赋值-Current code line number, will be assigned automatically</param>
        void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
    }
}
