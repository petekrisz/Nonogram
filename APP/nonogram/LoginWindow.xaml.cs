using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using nonogram.Common;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7CC"));
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#660000"));
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit the application?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                CopyFileHelper.CopyCsvFilesOnExit();
                Application.Current.Shutdown();
            }
        }


    }
}
