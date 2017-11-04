using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FFXIV.Framework.Extensions;

namespace FFXIV.Framework.Common
{
    /// <summary>
    /// Update Checker
    /// </summary>7
    public static class UpdateChecker
    {
        private static string GetLastestReleaseUrl(
            string productName)
        {
            var url = string.Empty;

            var asm = GetHojoringAssembly();
            if (asm != null)
            {
                dynamic hojoring = asm.CreateInstance("UpdateController");
                if (hojoring != null)
                {
                    url = hojoring.LastestReleaseUrl;
                }
            }

            return url;
        }

        private static Assembly GetHojoringAssembly()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dll = Path.Combine(dir, "ACT.Hojoring.dll");
            if (File.Exists(dll))
            {
                return Assembly.LoadFrom(dll);
            }

            return null;
        }

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

                // lastest releaseのURLを置き換える
                var url = GetLastestReleaseUrl(productName);
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
                var hojoringAsm = GetHojoringAssembly();
                if (hojoringAsm != null)
                {
                    currentAssembly = hojoringAsm;
                }

                var currentVersion = currentAssembly.GetName().Version;

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
