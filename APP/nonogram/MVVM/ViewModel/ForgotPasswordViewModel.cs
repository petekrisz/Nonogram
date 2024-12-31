using nonogram.Common;
using nonogram.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows;
using nonogram.MVVM.View;
using System.Diagnostics;

namespace nonogram.MVVM.ViewModel
{
    public class ForgotPasswordViewModel : ObservableObject
    {
        private string _email;
        private string _verificationCode;
        private string _enteredCode;
        private string _newPassword;
        private string _confirmPassword;
        private Timer _timer;
        private DateTime _codeExpirationTime;
        private readonly SmtpServer _smtpServer;

        private bool _isEmailInputVisible = true;
        private bool _isVerificationCodeVisible = false;
        private bool _isPasswordChangeVisible = false;
        private string _timerText;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string EnteredCode
        {
            get => _enteredCode;
            set => SetProperty(ref _enteredCode, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public bool IsEmailInputVisible
        {
            get => _isEmailInputVisible;
            set => SetProperty(ref _isEmailInputVisible, value);
        }

        public bool IsVerificationCodeVisible
        {
            get => _isVerificationCodeVisible;
            set => SetProperty(ref _isVerificationCodeVisible, value);
        }

        public bool IsPasswordChangeVisible
        {
            get => _isPasswordChangeVisible;
            set => SetProperty(ref _isPasswordChangeVisible, value);
        }

        public string TimerText
        {
            get => _timerText;
            set => SetProperty(ref _timerText, value);
        }

        public ICommand RequestNewPasswordCommand { get; }
        public ICommand VerifyCodeCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        public ForgotPasswordViewModel()
        {
            _smtpServer = new SmtpServer("smtp.mailtrap.io", 587, "your-username", "your-password");
            RequestNewPasswordCommand = new RelayCommand<object>(RequestNewPassword);
            VerifyCodeCommand = new RelayCommand<object>(VerifyCode);
            ChangePasswordCommand = new RelayCommand<object>(ChangePassword);
        }

        private async void RequestNewPassword(object parameter)
        {
            var forgotPasswordView = parameter as ForgotPasswordView;
            var Email = forgotPasswordView.EmailAddress.Text;

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Please enter your email address.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!UserExists(Email))
            {
                MessageBox.Show("The provided email address is not registered.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _verificationCode = GenerateVerificationCode();
            await _smtpServer.SendEmailAsync(Email, "Password Reset Code", $"Your password reset code is: {_verificationCode}");
            MessageBox.Show("A verification code has been sent to your email.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Information);

            Debug.WriteLine($"Verification code: {_verificationCode}");

            _codeExpirationTime = DateTime.Now.AddMinutes(15);
            StartTimer();

            IsEmailInputVisible = false;
            IsVerificationCodeVisible = true;
        }

        private bool UserExists(string email)
        {
            var dbManager = new DbManager();
            string query = "SELECT COUNT(*) FROM USER WHERE Email = @Email";
            var parameters = new Dictionary<string, object> { { "@Email", email } };
            var result = dbManager.ExecuteQuery(query, parameters);
            return Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void StartTimer()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var remainingTime = _codeExpirationTime - DateTime.Now;
            if (remainingTime <= TimeSpan.Zero)
            {
                _timer.Stop();
                _verificationCode = null;
                TimerText = "00:00";
                MessageBox.Show("The verification code has expired.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Error);
                IsVerificationCodeVisible = false;
                IsEmailInputVisible = true;
            }
            else
            {
                TimerText = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                Console.WriteLine($"Timer updated: {TimerText}"); // Debug statement
            }
        }

        private void VerifyCode(object parameter)
        {
            if (EnteredCode == _verificationCode)
            {
                MessageBox.Show("The code is correct. You can now change your password.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowPasswordChangeFields();
            }
            else
            {
                MessageBox.Show("The provided code is incorrect.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Error);
                EnteredCode = string.Empty;
            }
        }

        private void ShowPasswordChangeFields()
        {
            // Logic to show password change fields
        }

        private void ChangePassword(object parameter)
        {
            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dbManager = new DbManager();
            string hashedPassword = HashHelper.ComputeSha256Hash(NewPassword);
            string query = "UPDATE USER SET Password = @Password WHERE Email = @Email";
            var parameters = new Dictionary<string, object>
            {
                { "@Password", hashedPassword },
                { "@Email", Email }
            };
            dbManager.ExecuteNonQuery(query, parameters);

            MessageBox.Show("Your password has been changed successfully.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            // Logic to navigate back to LoginViewModel
        }
    }
}
