using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FFXIV.Framework.Extensions;
using NLog;

namespace FFXIV.Framework.Common
{
    /// <summary>
    /// Update Checker
    /// </summary>7
    public static class UpdateChecker
    {
        #region Logger

        private static Logger Logger => AppLog.DefaultLogger;

        #endregion Logger

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static dynamic GetHojoring()
        {
            var obj = default(object);

            try
            {
                obj = new ACT.Hojoring.Common.Hojoring();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                obj = null;
            }

            return obj;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetLastestReleaseUrl()
        {
            var url = string.Empty;

            var hojoring = GetHojoring();
            if (hojoring != null)
            {
                url = hojoring.LastestReleaseUrl;
            }

            return url;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Version GetHojoringVersion()
        {
            var ver = default(Version);

            try
            {
                var hojoring = GetHojoring();
                if (hojoring != null)
                {
                    ver = hojoring.Version;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ver = null;
            }

            return ver;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ShowSplash()
        {
            try
            {
                var hojoring = GetHojoring();
                if (hojoring != null)
                {
                    var ver = hojoring.Version as Version;
                    if (ver != null)
                    {
                        Logger.Trace($"*** Hojoring v{ver.Major}.{ver.Minor}.{ver.Revision} ***");
                    }

                    hojoring.ShowSplash();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// チェック済み辞書
        /// </summary>
        private static readonly Dictionary<string, bool> checkedDictinary = new Dictionary<string, bool>();

        /// <summary>
        /// アップデートを行う
        /// </summary>
        /// <returns>メッセージ</returns>
        public static string Update(
            string productName,
            string lastestReleaseUrl,
            Assembly currentAssembly)
        {
            var r = string.Empty;

            try
            {
                var html = string.Empty;

                // HojoringのURLに置き換える？
                var url = GetLastestReleaseUrl();
                if (!string.IsNullOrEmpty(url))
                {
                    lastestReleaseUrl = url;
                }

                // lastest releaseページを取得する
                using (var wc = new WebClient())
                {
                    using (var stream = wc.OpenRead(lastestReleaseUrl))
                    using (var sr = new StreamReader(stream))
                    {
                        html = sr.ReadToEnd();
                    }
                }

                var lastestReleaseVersion = string.Empty;

                // バージョン情報（lastest releaseのタイトル）を抜き出すパターンを編集する
                var pattern = string.Empty;
                pattern += @"<h1 class=""release-title"".*?>.*?";
                pattern += @"<a href="".*?"".*?>";
                pattern += @"(?<ReleaseTitle>.*?)";
                pattern += @"</a>.*?";
                pattern += @"</h1>";

                // バージョン情報を抜き出す
                var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                for (Match m = regex.Match(html); m.Success; m = m.NextMatch())
                {
                    lastestReleaseVersion = m.Groups["ReleaseTitle"].Value.Trim();
                }

                if (string.IsNullOrWhiteSpace(lastestReleaseVersion))
                {
                    return r;
                }

                // 現在のバージョンを取得する
                var currentVersion = currentAssembly.GetName().Version;

                // Hororingのバージョンに置き換える？
                var hojoringVer = GetHojoringVersion();
                if (hojoringVer != null)
                {
                    currentVersion = hojoringVer;
                }

                // バージョンを比較する
                if (!lastestReleaseVersion.ContainsIgnoreCase("FINAL"))
                {
                    var values = lastestReleaseVersion.Replace("v", string.Empty).Split('.');
                    var remoteVersion = new Version(
                        values.Length > 0 ? int.Parse(values[0]) : 0,
                        values.Length > 1 ? int.Parse(values[1]) : 0,
                        0,
                        values.Length > 2 ? int.Parse(values[2]) : 0);

                    if (remoteVersion <= currentVersion)
                    {
                        return r;
                    }
                }

                // チェック済み？
                if (checkedDictinary.ContainsKey(lastestReleaseUrl) &&
                    checkedDictinary[lastestReleaseUrl])
                {
                    return r;
                }

                // このURLはチェック済みにする
                checkedDictinary[lastestReleaseUrl] = true;

                var prompt = string.Empty;
                prompt += $"{productName} new version released !" + Environment.NewLine;
                prompt += "now: " + "v" + currentVersion.Major.ToString() + "." + currentVersion.Minor.ToString() + "." + currentVersion.Revision.ToString() + Environment.NewLine;
                prompt += "new: " + lastestReleaseVersion + Environment.NewLine;
                prompt += Environment.NewLine;
                prompt += "Do you want to download ?";

                if (MessageBox.Show(prompt, productName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                    DialogResult.Yes)
                {
                    return r;
                }

                Process.Start(lastestReleaseUrl);
            }
            catch (Exception ex)
            {
                r = $"Update Checker Error !\n\n{ex.ToString()}";
            }

            return r;
        }
    }
}
