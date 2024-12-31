using nonogram.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (DataContext as LoginViewModel)?.LoginCommand.Execute(this);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as LoginViewModel)?.LoginCommand.Execute(this);
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7CC"));
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#998000"));
        }

        private void ForgotPasswordLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (DataContext as LoginViewModel)?.ForgotPasswordCommand.Execute(null);
        }
    }
}
