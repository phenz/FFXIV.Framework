using System;
using System.Collections.Generic;
using System.IO;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace FFXIV.Framework.Common
{
    /// <summary>
    /// WavePlayerTypes
    /// </summary>
    public enum WavePlayerTypes
    {
        WaveOut,
        DirectSound,
        WASAPI,
        ASIO,
    }

    public class WavePlayer
    {
        #region Singleton

        private static WavePlayer instance;

        public static WavePlayer Instance =>
            instance ?? (instance = new WavePlayer());

        public static void Free()
        {
            instance = null;
        }

        #endregion Singleton

        /// <summary>
        /// 再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevices(
            WavePlayerTypes playerType)
        {
            var list = default(List<PlayDevice>);

            switch (playerType)
            {
                case WavePlayerTypes.WaveOut:
                    list = EnumlateDevicesByWaveOut();
                    break;

                case WavePlayerTypes.DirectSound:
                    list = EnumlateDevicesByDirectSoundOut();
                    break;

                case WavePlayerTypes.WASAPI:
                    list = EnumlateDevicesByWasapiOut();
                    break;

                case WavePlayerTypes.ASIO:
                    list = EnumlateDevicesByAsioOut();
                    break;
            }

            if (list != null)
            {
                list.Add(PlayDevice.DiscordPlugin);
            }

            return list;
        }

        /// <summary>
        /// AsioOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByAsioOut()
        {
            var list = new List<PlayDevice>();

            foreach (var name in AsioOut.GetDriverNames())
            {
                list.Add(new PlayDevice()
                {
                    ID = name,
                    Name = name,
                });
            }

            return list;
        }

        /// <summary>
        /// DirectSoundOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByDirectSoundOut()
        {
            var list = new List<PlayDevice>();

            foreach (var device in DirectSoundOut.Devices)
            {
                list.Add(new PlayDevice()
                {
                    ID = device.Guid.ToString(),
                    Name = device.Description,
                });
            }

            return list;
        }

        private static readonly MMDeviceEnumerator deviceEnumrator = new MMDeviceEnumerator();

        /// <summary>
        /// WasapiOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByWasapiOut()
        {
            var list = new List<PlayDevice>();

            foreach (var device in deviceEnumrator.EnumerateAudioEndPoints(
                DataFlow.Render,
                DeviceState.Active))
            {
                list.Add(new PlayDevice()
                {
                    ID = device.ID,
                    Name = device.FriendlyName,
                });
            }

            return list;
        }

        /// <summary>
        /// WaveOutから再生デバイスを列挙する
        /// </summary>
        /// <returns>再生デバイスのリスト</returns>
        public static List<PlayDevice> EnumlateDevicesByWaveOut()
        {
            var list = new List<PlayDevice>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var capabilities = WaveOut.GetCapabilities(i);
                list.Add(new PlayDevice()
                {
                    ID = i.ToString(),
                    Name = capabilities.ProductName,
                });
            }

            return list;
        }

        public void Play(
            string file,
            float volume = 1.0f,
            WavePlayerTypes playerType = WavePlayerTypes.WASAPI,
            string deviceID = null)
        {
            if (!File.Exists(file))
            {
                return;
            }

            var audio = new AudioFileReader(file)
            {
                Volume = volume
            };

            var player = this.CreatePlayer(playerType, deviceID);

            player.Init(audio);
            player.PlaybackStopped += (x, y) =>
            {
                audio.Dispose();
                player.Dispose();
                audio = null;
                player = null;
            };

            player.Play();
        }

        private const int PlayerLatencyWaveOut = 200;
        private const int PlayerLatencyDirectSoundOut = 200;
        private const int PlayerLatencyWasapiOut = 200;

        /// <summary>
        /// プレイヤーを生成する
        /// </summary>
        /// <param name="playerType">プレイヤーの種類</param>
        /// <param name="deviceID">再生デバイス</param>
        /// <returns>
        /// プレイヤー</returns>
        public IWavePlayer CreatePlayer(
            WavePlayerTypes playerType = WavePlayerTypes.WASAPI,
            string deviceID = null)
        {
            var player = default(IWavePlayer);
            switch (playerType)
            {
                case WavePlayerTypes.WaveOut:
                    player = deviceID == null ?
                        new WaveOut() :
                        new WaveOut()
                        {
                            DeviceNumber = int.Parse(deviceID),
                            DesiredLatency = PlayerLatencyWaveOut,
                        };
                    break;

                case WavePlayerTypes.DirectSound:
                    player = deviceID == null ?
                        new DirectSoundOut() :
                        new DirectSoundOut(
                            Guid.Parse(deviceID),
                            PlayerLatencyDirectSoundOut);
                    break;

                case WavePlayerTypes.WASAPI:
                    player = deviceID == null ?
                        new WasapiOut() :
                        new WasapiOut(
                            deviceEnumrator.GetDevice(deviceID),
                            AudioClientShareMode.Shared,
                            false,
                            PlayerLatencyWasapiOut);
                    break;

                case WavePlayerTypes.ASIO:
                    player = deviceID == null ?
                        new AsioOut() :
                        new AsioOut(deviceID);
                    break;
            }

            return player;
        }
    }

    /// <summary>
    /// 再生デバイス
    /// </summary>
    public class PlayDevice
    {
        public const string DiscordDeviceID = "DISCORD";

        public readonly static PlayDevice DiscordPlugin = new PlayDevice()
        {
            ID = DiscordDeviceID,
            Name = "Use Discord",
        };

        /// <summary>
        /// デバイスのID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// デバイス名
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
            => this.Name;
    }
}
