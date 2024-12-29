using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using nonogram.Common;

namespace nonogram
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("All fields are required.", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 6 characters long and contain at least one number and one uppercase letter.", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Feltételezzük, hogy a felhasználónév az e-mail cím domain része nélkül kerül megadásra
            string username = email.Split('@')[0];
            int tokens = 50; // Bónusz tokenek
            string hashedPassword = HashHelper.ComputeSha256Hash(password);

            string timeOfRegistration = DateTime.Now.ToString("yyyy. MM. dd. H:mm:ss");
            string newUser = $"{username};{hashedPassword};{firstName};{lastName};{email};{timeOfRegistration};0;{tokens};Avatar_{username}.png";

            File.AppendAllText("USER.csv", newUser + Environment.NewLine);

            MessageBox.Show("Registration successful! You have received 50 bonus tokens.", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private bool IsValidPassword(string password)
        {
            // A jelszónak legalább 6 karakter hosszúnak kell lennie, és tartalmaznia kell legalább egy számot és egy nagybetűt
            return password.Length >= 6 &&
                   password.Any(char.IsDigit) &&
                   password.Any(char.IsUpper);
        }
    }
}
