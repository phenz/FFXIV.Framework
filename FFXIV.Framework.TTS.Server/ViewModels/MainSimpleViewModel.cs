using System;
using System.Text;
using System.Windows.Input;
using FFXIV.Framework.Common;
using FFXIV.Framework.TTS.Server.Config;
using FFXIV.Framework.TTS.Server.Models;
using FFXIV.Framework.TTS.Server.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace FFXIV.Framework.TTS.Server.ViewModels
{
    public class MainSimpleViewModel :
        BindableBase
    {
        #region View

        public MainView View { get; set; }

        #endregion View

        public Settings Config => Settings.Instance;

        private string ipcChannelUri;
        private DelegateCommand refreshIPCChannelCommand;
        private DelegateCommand startCevioCommand;

        public MainSimpleViewModel()
        {
            AppLog.AppendedLog += (s, e) =>
            {
                WPFHelper.BeginInvoke(() =>
                {
                    this.RaisePropertyChanged(nameof(this.Messages));
                });
            };
        }

        public string IPCChannelUri
        {
            get => this.ipcChannelUri;
            set => this.SetProperty(ref this.ipcChannelUri, value);
        }

        public string Messages => AppLog.Log.ToString();

        public ICommand RefreshIPCChannelCommand => (this.refreshIPCChannelCommand ?? (this.refreshIPCChannelCommand = new DelegateCommand(() =>
        {
            RemoteTTSServer.Instance.Close();
            RemoteTTSServer.Instance.Open();

            this.View.ShowMessage(
                "Refresh IPC Channel",
                "Done.");
        })));

        public ICommand StartCevioCommand => (this.startCevioCommand ?? (this.startCevioCommand = new DelegateCommand(() =>
        {
            try
            {
                CevioModel.Instance.StartCevio();

                this.View.ShowMessage(
                    "Start CeVIO Creative Studio",
                    "Done.");
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Error.");
                sb.AppendLine();
                sb.AppendLine(ex.ToString());

                this.View.ShowMessage(
                    "Start CeVIO Creative Studio",
                    sb.ToString());
            }
        })));
    }
}
