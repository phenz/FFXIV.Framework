using System.IO;
using System.Linq;
using CeVIO.Talk.RemoteService;
using FFXIV.Framework.Common;
using FFXIV.Framework.TTS.Common.Models;
using FFXIV.Framework.TTS.Server.Properties;
using NAudio.Wave;
using NLog;

namespace FFXIV.Framework.TTS.Server.Models
{
    public class CevioModel
    {
        #region Singleton

        private static CevioModel instance = new CevioModel();
        public static CevioModel Instance => instance;

        #endregion Singleton

        #region Logger

        private Logger logger = AppLog.DefaultLogger;

        #endregion Logger

        private Talker cevioTalker;

        #region Start / Kill

        public void KillCevio()
        {
            if (ServiceControl.IsHostStarted)
            {
                ServiceControl.CloseHost(HostCloseMode.Interrupt);
                this.logger.Info($"CeVIO Remote Service, CloseHost.");
            }

            if (this.cevioTalker != null)
            {
                this.cevioTalker = null;
            }
        }

        public void StartCevio()
        {
            if (!ServiceControl.IsHostStarted)
            {
                ServiceControl.StartHost(false);
                this.logger.Info($"CeVIO Remote Service, StartHost.");
            }

            if (this.cevioTalker == null)
            {
                this.cevioTalker = new Talker();
            }
        }

        #endregion Start / Kill

        public CevioTalkerModel GetCevioTalker()
        {
            this.StartCevio();

            if (this.cevioTalker == null)
            {
                return null;
            }

            var talkerModel = new CevioTalkerModel()
            {
                Volume = this.cevioTalker.Volume,
                Speed = this.cevioTalker.Speed,
                Tone = this.cevioTalker.Tone,
                Alpha = this.cevioTalker.Alpha,
                ToneScale = this.cevioTalker.ToneScale,
                Cast = this.cevioTalker.Cast,
                AvailableCasts = Talker.AvailableCasts,

                Components = (
                    from x in this.cevioTalker.Components
                    select new CevioTalkerModel.CevioTalkerComponent()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Value = x.Value,
                    }).ToArray(),
            };

            return talkerModel;
        }

        public void SetCevioTalker(
            CevioTalkerModel talkerModel)
        {
            this.StartCevio();

            if (this.cevioTalker == null)
            {
                return;
            }

            this.cevioTalker.Volume = talkerModel.Volume;
            this.cevioTalker.Speed = talkerModel.Speed;
            this.cevioTalker.Tone = talkerModel.Tone;
            this.cevioTalker.Alpha = talkerModel.Alpha;
            this.cevioTalker.ToneScale = talkerModel.ToneScale;
            this.cevioTalker.Cast = talkerModel.Cast;

            foreach (var com in this.cevioTalker.Components)
            {
                var src = talkerModel.Components.FirstOrDefault(x => x.Id == com.Id);
                if (src != null)
                {
                    com.Value = src.Value;
                }
            }
        }

        public void TextToWave(
            string textToSpeak,
            string waveFileName)
        {
            if (string.IsNullOrEmpty(textToSpeak))
            {
                return;
            }

            this.StartCevio();

            var tempWave = Path.GetTempFileName();

            try
            {
                var result = this.cevioTalker.OutputWaveToFile(
                    textToSpeak,
                    tempWave);

                if (result)
                {
                    FileHelper.CreateDirectory(waveFileName);

                    if (Settings.Default.CevioGain != 1.0)
                    {
                        // ささらは音量が小さめなので増幅する
                        using (var reader = new WaveFileReader(tempWave))
                        {
                            var prov = new VolumeWaveProvider16(reader)
                            {
                                Volume = Settings.Default.CevioGain
                            };

                            WaveFileWriter.CreateWaveFile(
                                waveFileName,
                                prov);
                        }
                    }
                    else
                    {
                        File.Move(tempWave, waveFileName);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempWave))
                {
                    File.Delete(tempWave);
                }
            }
        }
    }
}
