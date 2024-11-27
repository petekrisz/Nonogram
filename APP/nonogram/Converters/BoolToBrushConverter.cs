using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Diagnostics;

namespace nonogram.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine($"BoolToBrushConverter called. Value: {value}, Parameter: {parameter}");
            bool isHighlighted = value is bool && (bool)value;

            if (parameter as string == "ImageCell")
            {
                return isHighlighted
                    ? new SolidColorBrush(Color.FromRgb(200, 200, 255)) // Highlight color for ImageCell
                    : new SolidColorBrush(Color.FromRgb(255, 247, 204)); // Default #FFF7CC
            }

            if (parameter as string == "Table")
            {
                return isHighlighted
                    ? new SolidColorBrush(Color.FromRgb(211, 211, 211)) // Highlight color for Row/Column
                    : new SolidColorBrush(Color.FromRgb(192, 192, 192)); // Default LightGray
            }

            return new SolidColorBrush(Colors.Transparent);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
