using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace nonogram.MVVM.View
{
    public partial class ResetPasswordWindow : Window
    {
        private string userEmail;

        public ResetPasswordWindow(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Minden mező kitöltése kötelező!", "Új jelszó beállítása", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("A két jelszó nem egyezik!", "Új jelszó beállítása", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("A jelszónak legalább 8 karakter hosszúnak kell lennie, tartalmaznia kell legalább 1 nagybetűt és 1 számot.", "Új jelszó beállítása", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                UpdatePassword(userEmail, newPassword);
                MessageBox.Show("A jelszó sikeresen megváltozott!", "Új jelszó beállítása", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a jelszó frissítésekor: {ex.Message}", "Új jelszó beállítása", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 8)
                return false;
            if (!password.Any(char.IsUpper))
                return false;
            if (!password.Any(char.IsDigit))
                return false;
            return true;
        }

        private void UpdatePassword(string email, string newPassword)
        {
            string filePath = "users.csv";
            var lines = File.ReadAllLines(filePath).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 5 && parts[2] == email)
                {
                    parts[3] = newPassword;
                    lines[i] = string.Join(",", parts);
                    break;
                }
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
