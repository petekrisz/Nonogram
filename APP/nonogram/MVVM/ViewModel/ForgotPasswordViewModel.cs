using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class ForgotPasswordViewModel : ObservableObject
    {
        private string _email;
        private string _verificationCode;
        private Timer _timer;
        private DateTime _codeExpirationTime;
        private readonly SmtpServer _smtpServer;

        private bool _isEmailInputVisible = true;
        private bool _isVerificationCodeVisible = false;
        private bool _isPasswordChangeVisible = false;
        private string _timerText;

        //public string Email
        //{
        //    get => _email;
        //    set => SetProperty(ref _email, value);
        //}

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

        private readonly LoginViewModel _loginViewModel;

        public ICommand RequestNewPasswordCommand { get; }
        public ICommand VerifyCodeCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public ForgotPasswordViewModel(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
            _smtpServer = new SmtpServer("smtp.mailersend.net", 587, "MS_GfqEet@trial-0r83ql3z0om4zw1j.mlsender.net", "rBibxwfIKwMybJBF");
            RequestNewPasswordCommand = new RelayCommand<object>(RequestNewPassword);
            //Debug.WriteLine($"ForgotPasswordViewModel: RequestNewPasswordCommand: {RequestNewPasswordCommand.GetHashCode()}");
            VerifyCodeCommand = new RelayCommand<object>(VerifyCode);
            ChangePasswordCommand = new RelayCommand<object>(ChangePassword);
            NavigateToLoginCommand = new RelayCommand<object>(_ => LoginNavigationHelper.NavigateToLoginWindow(_loginViewModel));
        }

        private async void RequestNewPassword(object parameter)
        {
            var forgotPasswordView = parameter as ForgotPasswordView;
            var Email = forgotPasswordView.EmailAddress.Text;
            //Debug.WriteLine($"Email: {Email}");

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

            _email = Email;
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
            const string query = "SELECT COUNT(*) FROM USER WHERE Email = @Email";
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
                //Console.WriteLine($"Timer updated: {TimerText}");
            }
        }

        private void VerifyCode(object parameter)
        {
            var forgotPasswordView = parameter as ForgotPasswordView;
            var enteredCode = forgotPasswordView.CodeBox.Text;

            if (enteredCode == _verificationCode)
            {
                MessageBox.Show("The code is correct. You can now change your password.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Information);
                IsVerificationCodeVisible = false;
                IsPasswordChangeVisible = true;
            }
            else
            {
                MessageBox.Show("The provided code is incorrect.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Error);
                forgotPasswordView.CodeBox.Text = string.Empty;
            }
        }

        private void ChangePassword(object parameter)
        {
            var forgotPasswordView = parameter as ForgotPasswordView;
            var newPassword_1 = forgotPasswordView.PasswordBox_1.Password;
            var newPassword_2 = forgotPasswordView.PasswordBox_2.Password;

            if (string.IsNullOrWhiteSpace(newPassword_1) || string.IsNullOrWhiteSpace(newPassword_2))
            {
                MessageBox.Show("Please enter password twice!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newPassword_1 != newPassword_2)
            {
                MessageBox.Show("Entered passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!PasswordValidator.IsValidPassword(newPassword_1) || !PasswordValidator.IsValidPassword(newPassword_2))
            {
                MessageBox.Show("Password must be at least 6 characters long and contain at least one number and one uppercase letter!", "Password Change", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Debug.WriteLine($"ChangePassword Email: {_email}");

            var hashedPassword = HashHelper.ComputeSha256Hash(newPassword_1);
            const string query = "UPDATE USER SET Password = @Password WHERE Email = @Email";
            var parameters = new Dictionary<string, object>
            {
                { "@Password", hashedPassword },
                { "@Email", _email } // Use the _email field instead of Email
            };

            var dbManager = new DbManager();
            dbManager.ExecuteNonQuery(query, parameters);

            MessageBox.Show("Your password has been changed successfully.", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Information);
            LoginNavigationHelper.NavigateToLoginWindow(_loginViewModel);
        }
    }
}
