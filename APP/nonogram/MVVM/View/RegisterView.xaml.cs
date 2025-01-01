using nonogram.MVVM.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Opacity == 0.5)
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                switch (textBox.Name)
                {
                    case "UsernameTextBox":
                        textBox.Text = "Username";
                        break;
                    case "FirstNameTextBox":
                        textBox.Text = "First Name";
                        break;
                    case "LastNameTextBox":
                        textBox.Text = "Last Name";
                        break;
                    case "EmailTextBox":
                        textBox.Text = "E-mail address";
                        break;
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as RegisterViewModel;
            viewModel?.RegisterCommand.Execute(this);
        }
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.NavigateToLoginCommand.Execute(null);
            }
        }
    }
}
