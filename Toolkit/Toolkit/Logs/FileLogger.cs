using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Toolkit.Logs
{
    /// <summary>
    /// 文件日志记录器，将日志记录到本地文件
    /// File logger, record log into local file.
    /// </summary>
    public class FileLogger : LoggerBase, IDisposable
    {
        #region fields
        /// <summary>
        /// 日志记录缓存，为了<see cref="ILogger"/>接口能够快速响应，调用接口时不会同时
        /// 将日志写入到文件，要异步执行写入操作，就需要先将日志缓存
        /// Log cache, for <see cref="ILogger"/> can have a fast response capability, 
        /// call interface will not write content to local file at the same time, for 
        /// write content asynchronously, must cache log firstly.
        /// </summary>
        private ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        /// <summary>
        /// 日志文件写入IO-The IO to write log
        /// </summary>
        private Stream _logWriter;
        /// <summary>
        /// <see cref="WriteLog"/>函数是否正在运行，1:正在运行，0:没有运行
        /// Is <see cref="WriteLog"/> function running, 1:running, 0:not running
        /// </summary>
        private int _isWriteLogRunning;
        #endregion

        #region constructors
        public FileLogger(string logPath)
        {
            // .Net没有异步的函数来打开文件，这里也消耗不了多少时间，所以暂时以同步的方式创建这个文件
            // .Net have no api to open file asnychronously and create file won't take too
            // much time, so here use sync way to create file temporarily.
            _logWriter = new FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, true);
        }
        #endregion

        #region properties
        #region LastError
        private Exception _lastError;
        /// <summary>
        /// 最新一次写入日志到本地文件遇到的异常
        /// Last exception for write log to local file.
        /// </summary>
        public Exception LastError
        {
            get { return this._lastError; }
        }
        #endregion
        #endregion

        #region public methods
        public void Dispose()
        {
            _logWriter.Dispose();
        }
        #endregion

        #region private methods
        /// <summary>
        /// 将缓存在<see cref="_logQueue"/>中的日志写入到本地文件，这个函数保证线程安全
        /// Write the logs that in <see cref="_logQueue"/> into local file, this
        /// method is thread-safe
        /// </summary>
        private async void WriteLog()
        {
            // 如果本函数正在其他线程运行，则直接退出
            // If this function is running in other thread, exit.
            if (Interlocked.CompareExchange(ref _isWriteLogRunning, 1, 0) == 1) return;

            Encoding encoding = Encoding.UTF8;
            byte[] linebreakBytes = encoding.GetBytes(new[] { '\n' });
            bool isOccurredException = false;
            string log;
            while (_logQueue.TryDequeue(out log))
            {
                if (!string.IsNullOrEmpty(log))
                {
                    try
                    {
                        // 编码字符串
                        // Encoding log
                        int logByteLength = encoding.GetByteCount(log);
                        byte[] logBytes = new byte[logByteLength + linebreakBytes.Length];
                        encoding.GetBytes(log, 0, log.Length, logBytes, 0);
                        // 添加换行符
                        // Add linebreak
                        Array.Copy(linebreakBytes, 0, logBytes, logByteLength, linebreakBytes.Length);
                        // 写入文件
                        await _logWriter.WriteAsync(logBytes, 0, logBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        isOccurredException = true;
                        _lastError = ex;
                        // 如果出了异常则停止整个写入操作
                        // If occur exception, stop whole function
                        break;
                    }
                }
            }

            // 如果遇到了异常则多等待一秒，以免异常在短时间内频繁发生从而影响效率
            // If occurred exception, wait one second to avoid ocurr too
            // many exception in a short time to drop performance.
            if (isOccurredException) await Task.Delay(1000);

            // 设置本函数正在运行的标识为0，让其他线程或之后可以再次运行这个函数
            // Set this function running flag to 0, let other thread or next
            // call can run properly.
            Volatile.Write(ref _isWriteLogRunning, 0);
        }

        private void StartWriteLog()
        {
            if (Volatile.Read(ref _isWriteLogRunning) == 0)
            {
                Task.Run(new Action(WriteLog));
            }
        }

        protected override void OnDebug(string log)
        {
            _logQueue.Enqueue(log);
            StartWriteLog();
        }

        protected override void OnError(string log)
        {
            _logQueue.Enqueue(log);
            StartWriteLog();
        }

        protected override void OnInfo(string log)
        {
            _logQueue.Enqueue(log);
            StartWriteLog();
        }

        protected override void OnWarn(string log)
        {
            _logQueue.Enqueue(log);
            StartWriteLog();
        }
        #endregion
    }
}
