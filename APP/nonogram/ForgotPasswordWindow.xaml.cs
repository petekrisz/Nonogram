using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace nonogram.MVVM.View
{
    public partial class ForgotPasswordWindow : Window
    {
        private SmtpServer smtpServer = new SmtpServer("sandbox.smtp.mailtrap.io", 587, "8234a940325cf0", "ad1ad8bea15e86");
        private string verificationCode;

        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private async void RequestNewPassword_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Kérem adja meg az e-mail címét.", "Elfelejtett jelszó", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!UserExists(email))
            {
                MessageBox.Show("A megadott e-mail cím nem található.", "Elfelejtett jelszó", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            verificationCode = GenerateVerificationCode();
            await smtpServer.SendEmailAsync(email, "Jelszó visszaállítási kód", $"Az Ön jelszó visszaállítási kódja: {verificationCode}");

            MessageBox.Show("E-mailben elküldtük a jelszó visszaállításához szükséges kódot.", "Elfelejtett jelszó", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowVerificationCodeInput();
        }

        private bool UserExists(string email)
        {
            string filePath = "users.csv";
            if (!File.Exists(filePath))
            {
                return false;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 3 && parts[2] == email)
                {
                    return true;
                }
            }
            return false;
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void ShowVerificationCodeInput()
        {
            VerificationCodeTextBox.Visibility = Visibility.Visible;
            VerificationCodePlaceholder.Visibility = Visibility.Visible;
            VerifyCodeButton.Visibility = Visibility.Visible;
            RequestNewPasswordButton.Visibility = Visibility.Collapsed;
        }

        private void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (VerificationCodeTextBox.Text == verificationCode)
            {
                MessageBox.Show("A kód helyes. Most megváltoztathatja a jelszavát.", "Jelszó visszaállítás", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenResetPasswordWindow();
            }
            else
            {
                MessageBox.Show("A megadott kód helytelen.", "Jelszó visszaállítás", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenResetPasswordWindow()
        {
            ResetPasswordWindow resetPasswordWindow = new ResetPasswordWindow(EmailTextBox.Text);
            resetPasswordWindow.Show();
            this.Close();
        }
    }
}
