using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace FFXIV.Framework.Dialog.Views
{
    public static class ColorDialog
    {
        private static ColorDialogContent content = new ColorDialogContent();

        private static Dialog dialog = new Dialog()
        {
            Title = "Colors ...",
            Content = content,
            MaxWidth = 1280,
            MaxHeight = 770,
        };

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
            ColorDialog.dialog.Owner = null;
            ColorDialog.dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ColorDialog.dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return ColorDialog.dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            Window owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            ColorDialog.dialog.Owner = owner;
            ColorDialog.dialog.WindowStartupLocation = starupLocation;

            ColorDialog.dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return ColorDialog.dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            System.Windows.Forms.Form owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            ColorDialog.dialog.WindowStartupLocation = starupLocation;

            if (owner != null)
            {
                var helper = new WindowInteropHelper(ColorDialog.dialog);
                helper.Owner = owner.Handle;
            }

            ColorDialog.dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return ColorDialog.dialog.ShowDialog();
        }
    }
}
