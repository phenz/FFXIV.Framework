namespace FFXIV.Framework.TTS.Common.Models
{
    public interface ITTSModel
    {
        CevioTalkerModel GetCevioTalker();

        bool IsReady();

        void SetCevioTalker(CevioTalkerModel talkerModel);

        void TextToWave(
            TTSTypes ttsType,
            string textToSpeak,
            string waveFileName,
            int speed);
    }
}
