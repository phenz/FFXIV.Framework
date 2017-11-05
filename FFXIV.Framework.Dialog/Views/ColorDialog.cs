using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace FFXIV.Framework.Dialog.Views
{
    public static class ColorDialog
    {
        private static ColorDialogContent content = new ColorDialogContent();

        public static Color Color
        {
            get => ColorDialog.content.Color;
            set => ColorDialog.content.Color = value;
        }

        public static bool IgnoreAlpha
        {
            get => ColorDialog.content.IgnoreAlpha;
            set => ColorDialog.content.IgnoreAlpha = value;
        }

        public static bool? ShowDialog()
        {
            var dialog = new Dialog
            {
                Title = "Colors ...",
                Content = ColorDialog.content,
                MaxWidth = 1280,
                MaxHeight = 770,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            Window owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            var dialog = new Dialog
            {
                Title = "Colors ...",
                Content = ColorDialog.content,
                MaxWidth = 1280,
                MaxHeight = 770,
                Owner = owner,
                WindowStartupLocation = starupLocation,
            };

            dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            System.Windows.Forms.Form owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            var dialog = new Dialog
            {
                Title = "Colors ...",
                Content = ColorDialog.content,
                MaxWidth = 1280,
                MaxHeight = 770,
                WindowStartupLocation = starupLocation,
            };

            if (owner != null)
            {
                var helper = new WindowInteropHelper(dialog);
                helper.Owner = owner.Handle;
            }

            dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return dialog.ShowDialog();
        }
    }
}
