using System;
using System.Windows;
using System.Windows.Threading;

namespace FFXIV.Framework.Common
{
    public class WPFHelper
    {
        public static DispatcherOperation BeginInvoke(
            Action action,
            DispatcherPriority priority = DispatcherPriority.Background)
        {
            return Application.Current?.Dispatcher.BeginInvoke(
                action,
                priority);
        }

        public static void Invoke(
            Action action,
            DispatcherPriority priority = DispatcherPriority.Background)
        {
            Application.Current?.Dispatcher.Invoke(
                action,
                priority);
        }
    }
}
