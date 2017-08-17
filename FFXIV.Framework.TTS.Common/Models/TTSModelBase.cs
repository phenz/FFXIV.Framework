using System;

namespace FFXIV.Framework.TTS.Common.Models
{
    public abstract class TTSModelBase :
        MarshalByRefObject,
        ITTSModel
    {
        public abstract CevioTalkerModel GetCevioTalker();

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public bool IsReady() => true;

        public abstract void SetCevioTalker(CevioTalkerModel talkerModel);

        public abstract void TextToWave(TTSTypes ttsType, string textToSpeak, string waveFileName, int speed);
    }
}
