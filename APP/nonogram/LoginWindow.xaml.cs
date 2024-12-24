using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using nonogram.Common;
using nonogram.MVVM.View;
using nonogram.MVVM.ViewModel;

namespace nonogram
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Kérem adja meg a felhasználó nevét és jelszavát.", "Bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsValidUser(email, password, out string fullName, out int points, out int tokens))
            {
                MessageBox.Show("Sikeres bejelentkezés!", "Bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Information);

                MainViewModel mainViewModel = new MainViewModel
                {
                    UserFullName = fullName,
                    UserPoints = points,
                    UserTokens = tokens
                };

                MainWindow mainWindow = new MainWindow(mainViewModel); // Használjuk a MainViewModel-t az új MainWindow konstruktorában

                Application.Current.MainWindow = mainWindow; // Állítsuk be a MainWindow-t

                mainWindow.Show();
                this.Close(); // Bezárjuk a LoginWindow-t
            }
            else
            {
                MessageBox.Show("Hibás e-mail vagy jelszó!", "Bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.ShowDialog();
        }

        private void OpenRegistrationWindow_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        private bool IsValidUser(string email, string password, out string fullName, out int points, out int tokens)
        {
            fullName = null;
            points = 0;
            tokens = 0;
            var lines = File.ReadAllLines("USER.csv"); // Helyes elérési útvonal
            string hashedPassword = HashHelper.ComputeSha256Hash(password);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(';');
                if (parts.Length >= 5 && parts[4] == email && parts[1] == hashedPassword)
                {
                    fullName = $"{parts[2]} {parts[3]}";
                    points = int.Parse(parts[6]);
                    tokens = int.Parse(parts[7]);
                    return true;
                }
            }
            return false;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
