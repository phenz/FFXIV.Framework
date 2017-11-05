using System.Windows;
using System.Windows.Interop;
using FFXIV.Framework.Common;

namespace FFXIV.Framework.Dialog.Views
{
    public static class FontDialog
    {
        private static FontDialogContent content = new FontDialogContent();

        public static FontInfo Font
        {
            get => FontDialog.content.FontInfo;
            set => FontDialog.content.FontInfo = value;
        }

        public static bool? ShowDialog()
        {
            var dialog = new Dialog
            {
                Title = "Fonts ...",
                Content = FontDialog.content,
                MaxWidth = 1280,
                MaxHeight = 770,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

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
                Title = "Fonts ...",
                Content = FontDialog.content,
                Owner = owner,
                MaxWidth = 1280,
                MaxHeight = 770,
                WindowStartupLocation = starupLocation,
            };

            dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

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
                Title = "Fonts ...",
                Content = FontDialog.content,
                MaxWidth = 1280,
                MaxHeight = 770,
                WindowStartupLocation = starupLocation,
            };

            if (owner != null)
            {
                var helper = new WindowInteropHelper(dialog);
                helper.Owner = owner.Handle;
            }

            dialog.OkButton.Click += FontDialog.content.OKBUtton_Click;

            return dialog.ShowDialog();
        }
    }
}
