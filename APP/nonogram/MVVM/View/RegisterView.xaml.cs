using nonogram.MVVM.ViewModel;
using System;
using System.Diagnostics;
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
            PrivacyCheckBox.Checked += PrivacyCheckBox_Checked;
            PrivacyCheckBox.Unchecked += PrivacyCheckBox_Unchecked;
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#998000"));
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7CC"));
        }

        private void PrivacyNotice_Click(object sender, MouseButtonEventArgs e)
        {
            string pdfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB", "Adatkezelési Tájékoztató.pdf");
            Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
        }

        private void PrivacyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            (DataContext as RegisterViewModel).IsPrivacyChecked = true;
        }

        private void PrivacyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as RegisterViewModel).IsPrivacyChecked = false;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                switch (textBox.Name)
                {
                    case "UsernameTextBox":
                        if (textBox.Text == "Username")
                        {
                            textBox.Text = string.Empty;
                        }
                        break;
                    case "FirstNameTextBox":
                        if (textBox.Text == "First Name")
                        {
                            textBox.Text = string.Empty;
                        }
                        break;
                    case "LastNameTextBox":
                        if (textBox.Text == "Last Name")
                        {
                            textBox.Text = string.Empty;
                        }
                        break;
                    case "EmailTextBox":
                        if (textBox.Text == "E-mail address")
                        {
                            textBox.Text = string.Empty;
                        }
                        break;
                        // Add more cases for other TextBoxes if needed
                }
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
            //Debug.WriteLine("Register button clicked.");
            var viewModel = DataContext as RegisterViewModel;
            viewModel?.RegisterCommand.Execute(this);
            if (viewModel.IsPrivacyChecked)
            {
                viewModel?.RegisterCommand.Execute(this);
            }
            else
            {
                MessageBox.Show("You must agree to the privacy notice before registering.", "Privacy Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
