using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FFXIV.Framework.WPF.Views
{
    public static class OverlayExtensions
    {
        #region Win32 API

        public const int GWL_EXSTYLE = (-20);

        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        #endregion Win32 API

        /// <summary>
        /// オーバーレイの表示を切り替える
        /// </summary>
        /// <param name="overlay">overlay</param>
        /// <param name="overlayVisible"></param>
        /// <param name="newValue"></param>
        /// <param name="opacity"></param>
        /// <returns>
        /// 切り替わったか否か？</returns>
        public static bool SetOverlayVisible(
            this IOverlay overlay,
            ref bool overlayVisible,
            bool newValue,
            double opacity = 1.0d)
        {
            if (overlayVisible != newValue)
            {
                overlayVisible = newValue;
                if (overlayVisible)
                {
                    overlay.ShowOverlay(opacity);
                }
                else
                {
                    overlay.HideOverlay();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// オーバーレイとして表示させる
        /// </summary>
        /// <param name="overlay">overlay</param>
        public static void ShowOverlay(
            this IOverlay overlay,
            double opacity = 1.0d)
        {
            if (overlay is Window w)
            {
                w.Opacity = opacity;
                w.Topmost = true;
            }
        }

        /// <summary>
        /// オーバーレイとして隠す
        /// </summary>
        /// <param name="overlay">overlay</param>
        public static void HideOverlay(
            this IOverlay overlay)
        {
            if (overlay is Window w)
            {
                w.Opacity = 0;
                w.Topmost = false;
            }
        }

        /// <summary>
        /// アクティブにさせない
        /// </summary>
        /// <param name="x">Window</param>
        public static void ToNonActive(
            this Window window)
        {
            window.SourceInitialized += (s, e) =>
            {
                // Get this window's handle
                var hwnd = new WindowInteropHelper(window).Handle;

                // Change the extended window style to include WS_EX_TRANSPARENT
                var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

                SetWindowLong(
                    hwnd,
                    GWL_EXSTYLE,
                    extendedStyle | WS_EX_NOACTIVATE);
            };
        }

        /// <summary>
        /// Clickの透過を解除する
        /// </summary>
        /// <param name="x">対象のWindow</param>
        public static void ToNotTransparent(
            this Window window)
        {
            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            SetWindowLong(
                hwnd,
                GWL_EXSTYLE,
                extendedStyle & ~WS_EX_TRANSPARENT);
        }

        /// <summary>
        /// Click透過させる
        /// </summary>
        /// <param name="x">対象のWindow</param>
        public static void ToTransparent(
            this Window window)
        {
            // Get this window's handle
            var hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            SetWindowLong(
                hwnd,
                GWL_EXSTYLE,
                extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}
