using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using FFXIV.Framework.TTS.Common.Models;

namespace FFXIV.Framework.TTS.Common
{
    public abstract class TTSServerBase
    {
        public const string IPCChannelName = "FFXIV.Framework.TTS.Server";
        public const string IPCRemoteObjectName = "TTSModel";

        protected TTSModelBase remoteObject;
        protected IpcServerChannel serverChannel;

        public static string TTSServerUri => $"ipc://{IPCChannelName}/{IPCRemoteObjectName}";
        public ITTSModel TTSModel => this.remoteObject as ITTSModel;

        public virtual void Close()
        {
            if (this.serverChannel != null)
            {
                ChannelServices.UnregisterChannel(this.serverChannel);
                this.serverChannel = null;
            }
        }

        public virtual void Open()
        {
            this.serverChannel = new IpcServerChannel(IPCChannelName);

            ChannelServices.RegisterChannel(this.serverChannel, false);
            if (this.remoteObject != null)
            {
                RemotingServices.Marshal(this.remoteObject, IPCRemoteObjectName, typeof(ITTSModel));
            }
        }
    }
}
