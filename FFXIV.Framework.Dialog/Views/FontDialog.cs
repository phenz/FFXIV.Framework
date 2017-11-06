using System.Windows;
using System.Windows.Interop;
using FFXIV.Framework.Common;

namespace FFXIV.Framework.Dialog.Views
{
    public static class FontDialog
    {
        private static FontDialogContent content = new FontDialogContent();

        private static Dialog Dialog => new Dialog()
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
            FontDialog.Dialog.Owner = null;
            FontDialog.Dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            FontDialog.Dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return FontDialog.Dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            Window owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            FontDialog.Dialog.Owner = null;
            FontDialog.Dialog.WindowStartupLocation = starupLocation;

            FontDialog.Dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return FontDialog.Dialog.ShowDialog();
        }

        public static bool? ShowDialog(
            System.Windows.Forms.Form owner)
        {
            var starupLocation = owner != null ?
                WindowStartupLocation.CenterOwner :
                WindowStartupLocation.CenterScreen;

            FontDialog.Dialog.Owner = null;
            FontDialog.Dialog.WindowStartupLocation = starupLocation;

            if (owner != null)
            {
                var helper = new WindowInteropHelper(FontDialog.Dialog);
                helper.Owner = owner.Handle;
            }

            FontDialog.Dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return FontDialog.Dialog.ShowDialog();
        }
    }
}
