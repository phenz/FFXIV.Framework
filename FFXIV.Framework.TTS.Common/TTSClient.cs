using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using FFXIV.Framework.TTS.Common.Models;

namespace FFXIV.Framework.TTS.Common
{
    public class TTSClient
    {
        private const string TTSServerName = "FFXIV.Framework.TTS.Server";

        #region Singleton

        private static TTSClient instance = new TTSClient();
        public static TTSClient Instance => instance;

        #endregion Singleton

        private IpcClientChannel clientChannel;
        private TTSModelBase remoteObject;

        public ITTSModel TTSModel => this.remoteObject as ITTSModel;

        public virtual void Close()
        {
            if (this.clientChannel != null)
            {
                ChannelServices.UnregisterChannel(this.clientChannel);
                this.clientChannel = null;
            }

            if (this.remoteObject != null)
            {
                this.remoteObject = null;
            }
        }

        public virtual void Open()
        {
            const double ConnectionTimeout = 30;

            this.clientChannel = new IpcClientChannel();
            ChannelServices.RegisterChannel(this.clientChannel, false);

            this.remoteObject = (TTSModelBase)Activator.GetObject(
                typeof(TTSModelBase),
                TTSServerBase.TTSServerUri);

            // 通信の確立を待つ
            Exception exception = null;
            var sw = Stopwatch.StartNew();
            do
            {
                try
                {
                    Thread.Sleep(100);
                    if (this.TTSModel.IsReady())
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            } while (sw.Elapsed.TotalSeconds <= ConnectionTimeout);
            sw.Stop();

            if (exception != null)
            {
                throw exception;
            }
        }

        public Process GetTTSServerProcess()
        {
            return Process.GetProcessesByName(TTSServerName).FirstOrDefault();
        }

        public void ShutdownTTSServer()
        {
            var p = this.GetTTSServerProcess();
            if (p != null)
            {
                p.Kill();
            }
        }

        public void StartTTSServer()
        {
            var p = this.GetTTSServerProcess();
            if (p != null)
            {
                return;
            }

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var ttsServer = Path.Combine(
                dir,
                $"{TTSServerName}.exe");

            Process.Start(ttsServer);
        }
    }
}
