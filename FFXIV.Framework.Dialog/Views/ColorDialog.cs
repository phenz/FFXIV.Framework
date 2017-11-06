using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace FFXIV.Framework.Dialog.Views
{
    public static class ColorDialog
    {
        private static ColorDialogContent content = new ColorDialogContent();

        private static Dialog Dialog => new Dialog()
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
            ColorDialog.Dialog.Owner = null;
            ColorDialog.Dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ColorDialog.Dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return ColorDialog.Dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            Window owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            ColorDialog.Dialog.Owner = owner;
            ColorDialog.Dialog.WindowStartupLocation = starupLocation;

            ColorDialog.Dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return ColorDialog.Dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            System.Windows.Forms.Form owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            ColorDialog.Dialog.WindowStartupLocation = starupLocation;

            if (owner != null)
            {
                var helper = new WindowInteropHelper(ColorDialog.Dialog);
                helper.Owner = owner.Handle;
            }

            ColorDialog.Dialog.OkButton.Click += (s, e) => ColorDialog.content.Apply();

            return ColorDialog.Dialog.ShowDialog();
        }
    }
}
