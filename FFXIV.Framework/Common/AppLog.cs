using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets.Wrappers;

namespace FFXIV.Framework.Common
{
    public static class AppLog
    {
        private const int LogBufferSize = 1024;
        private const int LogBufferMargin = 16;
        public const string ChatLoggerName = "ChatLogger";
        public const string DefaultLoggerName = "DefaultLogger";
        public const string HojoringConfig = "ACT.Hojoring.NLog.config";

        public static readonly object locker = new object();
        public static readonly List<AppLogEntry> logBuffer = new List<AppLogEntry>(LogBufferSize + LogBufferMargin);

        public delegate void AppendedLogEventHandler(object sender, AppendedLogEventArgs e);

        public static event AppendedLogEventHandler AppendedLog;

        /// <summary>
        /// ChatLog用のLogger
        /// </summary>
        public static Logger ChatLogger => LogManager.GetLogger(ChatLoggerName);

        /// <summary>
        /// 通常Logger
        /// </summary>
        public static Logger DefaultLogger => LogManager.GetLogger(DefaultLoggerName);

        public static StringBuilder Log
        {
            get
            {
                var sb = new StringBuilder();

                lock (AppLog.locker)
                {
                    sb.Append(
                        AppLog.logBuffer
                        .Aggregate<AppLogEntry, string>(
                            string.Empty, (x, y) =>
                            $"{x.ToString()}\n{y.ToString()}"));
                }

                return sb;
            }
        }

        /// <summary>
        /// NLogから呼ばれるUI向けログ出力
        /// </summary>
        /// <param name="dateTime">日付</param>
        /// <param name="level">ログレベル</param>
        /// <param name="callsite">callsite</param>
        /// <param name="message">メッセージ</param>
        public static void AppendLog(
            string dateTime,
            string level,
            string callsite,
            string message)
        {
            DateTime d;
            DateTime.TryParse(dateTime, out d);

            var entry = new AppLogEntry()
            {
                DateTime = d,
                Level = level,
                CallSite = callsite,
                Message = message,
            };

            lock (AppLog.locker)
            {
                if (AppLog.logBuffer.Count > LogBufferSize)
                {
                    AppLog.logBuffer.RemoveRange(0, LogBufferMargin);
                }

                AppLog.logBuffer.Add(entry);
            }

            AppLog.OnAppendedLog(new AppendedLogEventArgs(entry));
        }

        /// <summary>
        /// すべてのBufferingTargetをFlushする
        /// </summary>
        public static void FlushAll()
        {
            LogManager.Configuration.AllTargets
                .OfType<BufferingTargetWrapper>()
                .ToList()
                .ForEach(b => b.Flush(c =>
                {
                }));
        }

        private static bool loaded;

        public static void LoadConfiguration(
            string fileName)
        {
            lock (locker)
            {
                if (loaded)
                {
                    return;
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                if (File.Exists(fileName))
                {
                    LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);
                    loaded = true;
                    return;
                }

                var dirs = new[]
                {
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                };

                foreach (var dir in dirs)
                {
                    var file = Path.Combine(dir, Path.GetFileName(fileName));
                    if (File.Exists(file))
                    {
                        LogManager.Configuration = new XmlLoggingConfiguration(file, true);
                        loaded = true;
                        return;
                    }
                }
            }
        }

        private static void OnAppendedLog(
            AppendedLogEventArgs e)
        {
            AppLog.AppendedLog?.Invoke(null, e);
        }
    }
}
