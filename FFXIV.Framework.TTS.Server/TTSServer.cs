using FFXIV.Framework.Common;
using FFXIV.Framework.TTS.Common;
using FFXIV.Framework.TTS.Server.Models;
using FFXIV.Framework.TTS.Server.Views;
using NLog;

namespace FFXIV.Framework.TTS.Server
{
    public class TTSServer :
        TTSServerBase
    {
        #region Singleton

        private static TTSServer instance = new TTSServer();
        public static TTSServer Instance => instance;

        #endregion Singleton

        #region Logger

        private Logger logger = AppLog.DefaultLogger;

        #endregion Logger

        public override void Close()
        {
            base.Close();

            YukkuriModel.Instance.Free();
            this.logger.Info($"IPC Channel Closed.");
        }

        public override void Open()
        {
            this.remoteObject = new TTSModel();
            base.Open();

            var uri = $"{this.serverChannel.GetChannelUri()}/{TTSServer.IPCRemoteObjectName}";
            MainView.Instance.ViewModel.IPCChannelUri = uri;

            YukkuriModel.Instance.Load();

            this.logger.Info($"IPC Channel Listened. Uri={uri}");
        }
    }
}
