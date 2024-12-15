using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Diagnostics;
using nonogram.MVVM.ViewModel;

namespace nonogram.Converters
{
    public class BoolToBrushConverter : IMultiValueConverter
    {
        // Convert method for handling multiple binding values (IsHighlighted, ClickState)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3)
            {
                bool isHighlighted = (bool)values[0];  // IsHighlighted value (first binding)
                int clickState = (int)values[1];  // ClickState value (second binding)
                Brush initialBackground = values[2] as Brush;  // InitialBackground (third binding)

                // If ClickState is 1, return the black background
                if (clickState == 1)
                    return Brushes.Black;

                // If highlighted, return highlight color
                if (isHighlighted)
                {
                        // Create a copy of the initial background brush with modified opacity
                        Brush transparentBrush = initialBackground.Clone();
                        transparentBrush.Opacity = 0.8;
                        return transparentBrush;
                }

                // If not highlighted, return the initial background
                return initialBackground ?? Brushes.Transparent;
            }

            // Default case: return transparent if something goes wrong
            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }





    //public class BoolToBrushConverter : IValueConverter
    //{
    //        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //        {
    //            if (value is bool isHighlighted && parameter is string context && targetType == typeof(Brush))
    //            {
    //                if (context == "ImageCell" && value is bool highlighted)
    //                {
    //                    // Access the InitialBackground property directly from the bound data context
    //                    return highlighted ? Brushes.WhiteSmoke : Brushes.Transparent;
    //                }

    //                // Other contexts (e.g., hints)
    //                if (context == "Table")
    //                {
    //                    return isHighlighted ? Brushes.LightGray : Brushes.Transparent;
    //                }
    //            }

    //            return Brushes.Transparent; // Default case
    //        }

    //        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //        {
    //            throw new NotImplementedException();
    //        }

    //    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    //{
    //    //    if (value is bool isHighlighted && parameter is string context && targetType == typeof(Brush))
    //    //    {
    //    //        // Get the InitialBackground from the DataContext
    //    //        var element = parameter as GridElement;

    //    //        // Return appropriate brush
    //    //        if (context == "ImageCell")
    //    //        {
    //    //            return isHighlighted ? Brushes.WhiteSmoke : element?.InitialBackground ?? Brushes.Transparent;
    //    //        }
    //    //        else if (context == "Table")
    //    //        {
    //    //            return isHighlighted ? Brushes.LightGray : Brushes.Transparent;
    //    //        }
    //    //    }

    //    //    // Default case
    //    //    return Brushes.Transparent;
    //    //}

    //    //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}
}
