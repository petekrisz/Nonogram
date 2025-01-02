using nonogram.MVVM.Model;
using nonogram.MVVM.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace nonogram.MVVM.View
{
    public partial class HelpTableView : UserControl
    {
        public HelpTableView()
        {
            InitializeComponent();
            //Debug.WriteLine("      HelpTableView loaded.");
            //Debug.WriteLine($"---> HelpTableView.DataContext: {DataContext?.GetHashCode()}");

        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            if (sender is CheckBox clickedCheckBox && clickedCheckBox.IsChecked == true)
            {
                foreach (var checkBox in FindVisualChildren<CheckBox>(this))
                {
                    if (checkBox != clickedCheckBox)
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void HelpTableView_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"---> HelpTableViewModel instance in HelpTableView: {DataContext?.GetHashCode()}");

            foreach (var option in (DataContext as HelpTableViewModel)?.HelpOptions ?? Enumerable.Empty<HelpOption>())
            {
                Debug.WriteLine($"---> HelpOption in UI: {option.TypeOfHelp} -> {option.Value} (Instance: {option.GetHashCode()})");
            }
        }
    }
}
