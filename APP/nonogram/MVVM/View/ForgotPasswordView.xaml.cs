using nonogram.MVVM.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ForgotPasswordView.xaml
    /// </summary>
    public partial class ForgotPasswordView : UserControl
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                switch (textBox.Name)
                {
                    case "EmailAddress":
                        if (textBox.Text == "E-mail address")
                        {
                            textBox.Text = string.Empty;
                        }
                        break;
                    case "CodeBox":
                        if (textBox.Text == "Enter the received code:")
                        {
                            textBox.Text = string.Empty;
                        }
                        break;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                switch (textBox.Name)
                {
                    case "EmailAddress":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            textBox.Text = "E-mail address";;
                        }
                        break;
                    case "CodeBox":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            textBox.Text = "Enter the received code:";
                        }
                        break;
                }
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ForgotPasswordViewModel viewModel)
            {
                viewModel.NavigateToLoginCommand.Execute(null);
            }
        }
    }
}
