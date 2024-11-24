using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace nonogram.Converters
{
    public class SizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 ||
                values[0] == DependencyProperty.UnsetValue ||
                values[1] == DependencyProperty.UnsetValue)
            {
                Debug.WriteLine("SizeConverter received UnsetValue.");
                return 0; // Default value to avoid errors
            }

            if (values[0] is int count && values[1] is double cellSize)
            {
                return count * cellSize;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

