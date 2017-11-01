using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FFXIV.Framework.WPF.Converters
{
    public class ColorOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is double opacity))
            {
                return value;
            }

            switch (value)
            {
                case Color color:
                    return Color.FromArgb((byte)(255 * opacity), color.R, color.G, color.B);

                case SolidColorBrush brush:
                    var baseColor = brush.Color;
                    return new SolidColorBrush(Color.FromArgb((byte)(255 * opacity), baseColor.R, baseColor.G, baseColor.B));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
