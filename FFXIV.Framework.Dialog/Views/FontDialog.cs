using System.Windows;
using System.Windows.Interop;
using FFXIV.Framework.Common;

namespace FFXIV.Framework.Dialog.Views
{
    public static class FontDialog
    {
        private static FontDialogContent content = new FontDialogContent();

        private static Dialog dialog = new Dialog()
        {
            Title = "Fonts ...",
            Content = content,
            MaxWidth = 1280,
            MaxHeight = 770,
        };

        public static FontInfo Font
        {
            get => FontDialog.content.FontInfo;
            set => FontDialog.content.FontInfo = value;
        }

        public static bool? ShowDialog()
        {
            FontDialog.dialog.Owner = null;
            FontDialog.dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            FontDialog.dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return FontDialog.dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            Window owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            FontDialog.dialog.Owner = null;
            FontDialog.dialog.WindowStartupLocation = starupLocation;

            FontDialog.dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return FontDialog.dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            System.Windows.Forms.Form owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            FontDialog.dialog.Owner = null;
            FontDialog.dialog.WindowStartupLocation = starupLocation;

            if (owner != null)
            {
                var helper = new WindowInteropHelper(FontDialog.dialog);
                helper.Owner = owner.Handle;
            }

            FontDialog.dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return FontDialog.dialog.ShowDialog();
        }
    }
}
