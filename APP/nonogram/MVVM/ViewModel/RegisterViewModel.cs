using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;

namespace nonogram.MVVM.ViewModel
{
    public class RegisterViewModel : ObservableObject
    {
        private bool _isRegistering;
        private bool _isPrivacyChecked; // Flag to prevent re-entry
        public bool IsPrivacyChecked
        {
            get => _isPrivacyChecked;
            set => SetProperty(ref _isPrivacyChecked, value);
        }
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private readonly SmtpServer _smtpServer;

        private readonly LoginViewModel _loginViewModel;

        public RegisterViewModel(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
            _smtpServer = new SmtpServer("smtp.mailersend.net", 587, "MS_GfqEet@trial-0r83ql3z0om4zw1j.mlsender.net", "rBibxwfIKwMybJBF");

            RegisterCommand = new RelayCommand<object>(Register, CanRegister);
            Debug.WriteLine($"RegisterViewModel: RegisterCommand: {RegisterCommand.GetHashCode()}");
            NavigateToLoginCommand = new RelayCommand<object>(parameter => LoginNavigationHelper.NavigateToLoginWindow(_loginViewModel));
        }

        private async void Register(object parameter)
        {
            if (_isRegistering)
            {
                Debug.WriteLine("Register method is already running. Exiting.");
                return;
            }

            _isRegistering = true; // Set the flag to prevent re-entry
            Debug.WriteLine("Register method started.");

            try
            {
                Debug.WriteLine(parameter == null ? "Parameter is null." : $"Parameter type: {parameter.GetType()}");
                var registerView = parameter as RegisterView;
                var userName = registerView.UsernameTextBox.Text;
                var firstName = registerView.FirstNameTextBox.Text;
                var lastName = registerView.LastNameTextBox.Text;
                var email = registerView.EmailTextBox.Text;
                var password_1 = registerView.PasswordBox_1.Password;
                var password_2 = registerView.PasswordBox_2.Password;

                Debug.WriteLine($"Registering user: {userName}, {firstName}, {lastName}, {email}");

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password_1) || string.IsNullOrWhiteSpace(password_2))
                {
                    MessageBox.Show("Please complete all input fields!", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!PasswordValidator.IsValidPassword(password_1) || !PasswordValidator.IsValidPassword(password_2))
                {
                    MessageBox.Show("Password must be at least 6 characters long and contain at least one number and one uppercase letter!", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (password_1 != password_2)
                {
                    MessageBox.Show("Entered passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Debug.WriteLine("Checking if username is taken...");
                if (UsernameValidator.IsUsernameTaken(userName))
                {
                    MessageBox.Show("This username is already taken. Please choose a different one!", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Debug.WriteLine("Checking if email is valid...");
                if (!EmailValidator.IsValidEmail(email))
                {
                    MessageBox.Show("Please enter a valid email address.");
                    return;
                }

                Debug.WriteLine("Checking if email is taken...");
                if (IsEmailTaken(email))
                {
                    var result = MessageBox.Show("A player is already registered with this e-mail address. Please use a different e-mail address or select the forgot password option. Do you want to use the forgot password option?", "Registration", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.Yes)
                    {
                        LoginNavigationHelper.NavigateToLoginWindow(_loginViewModel);
                    }
                    return;
                }

                int tokens = 50; // Bonus tokens
                string hashedPassword = HashHelper.ComputeSha256Hash(password_1);
                DateTime timeOfRegistration = DateTime.Now;

                Debug.WriteLine("Inserting user into USER table...");
                // Insert user into USER table
                var dbManager = new DbManager();
                string queryUser = "INSERT INTO USER (UserName, Password, FirstName, LastName, Email, TimeOfRegistration, Score, Tokens) " +
                               "VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @TimeOfRegistration, 0, @Tokens)";
                var parametersUser = new Dictionary<string, object>
                    {
                        { "@UserName", userName },
                        { "@Password", hashedPassword },
                        { "@FirstName", firstName },
                        { "@LastName", lastName },
                        { "@Email", email },
                        { "@TimeOfRegistration", timeOfRegistration },
                        { "@Tokens", tokens }
                    };
                dbManager.ExecuteNonQuery(queryUser, parametersUser);

                Debug.WriteLine("Inserting user into USERHELP table...");
                // Insert user into USERHELP table
                string queryUserHelp = "INSERT INTO USERHELP (UserName, H1, H3, H8, H13, L1, L3, Check3H, Erase) " +
                   "VALUES (@UserName, 0, 0, 0, 0, 0, 0, 0, 0)";
                var parametersUserHelp = new Dictionary<string, object>
                    {
                        { "@UserName", userName }
                    };
                dbManager.ExecuteNonQuery(queryUserHelp, parametersUserHelp);

                MessageBox.Show("Registration successful! You have received 50 bonus tokens.", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show("You will be directed to the Login Window where you can log in with your newly registered account.", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);

                // Send welcome email
                await SendWelcomeEmail(firstName, userName, email);

                // Clear any lingering state
                ClearState();

                // Navigate to login window
                LoginNavigationHelper.NavigateToLoginWindow(_loginViewModel);

                Debug.WriteLine("Registration successful. Back to loginView");
            }
            finally
            {
                _isRegistering = false; // Reset the flag
                Debug.WriteLine("Register method finished.");
            }
        }

        private bool CanRegister(object parameter)
        {
            return IsPrivacyChecked && !_isRegistering; // Check the flag
        }

        private async Task SendWelcomeEmail(string firstName, string userName, string email)
        {
            string subject = "Welcome to NonoGram!";
            string body = $"Dear {firstName},<br><br>We are pleased to welcome you to the NonoGram game and hope you're going to enjoy playing with us. Your username is: {userName}.<br><br>The Nonogram team.";
            await _smtpServer.SendEmailAsync(email, subject, body);
        }

        private bool IsEmailTaken(string email)
        {
            var dbManager = new DbManager();
            string query = "SELECT COUNT(*) FROM USER WHERE Email = @Email";
            var parameters = new Dictionary<string, object>
                {
                    { "@Email", email }
                };
            var result = dbManager.ExecuteQuery(query, parameters);
            return Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        private void ClearState()
        {
            // Clear any state related to the registration process
            IsPrivacyChecked = false;
        }
    }
    public static class PasswordValidator
    {
        public static bool IsValidPassword(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsDigit) &&
                   password.Any(char.IsUpper);
        }
    }
    public static class UsernameValidator
    {
        public static bool IsUsernameTaken(string username)
        {
            Debug.WriteLine($"Checking if username '{username}' is taken...");
            var dbManager = new DbManager();
            string query = "SELECT COUNT(*) FROM USER WHERE UserName = @UserName";
            var parameters = new Dictionary<string, object>
                {
                    { "@UserName", username }
                };
            var result = dbManager.ExecuteQuery(query, parameters);
            bool isTaken = Convert.ToInt32(result.Rows[0][0]) > 0;
            Debug.WriteLine($"Username '{username}' is taken: {isTaken}");
            return isTaken;
        }
    }

}
