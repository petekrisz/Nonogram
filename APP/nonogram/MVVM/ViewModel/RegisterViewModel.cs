using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MySqlX.XDevAPI.Common;
using System.Diagnostics;
using System.Windows.Controls;

namespace nonogram.MVVM.ViewModel
{
    internal class RegisterViewModel : ObservableObject
    {
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand<object>(Register);
            NavigateToLoginCommand = new RelayCommand<object>(parameter => NavigateToLoginWindow(parameter as RegisterView));
        }

        private void Register(object parameter)
        {
            Debug.WriteLine(parameter == null ? "Parameter is null." : $"Parameter type: {parameter.GetType()}");
            var registerView = parameter as RegisterView;
            var userName = registerView.UsernameTextBox.Text;
            var firstName = registerView.FirstNameTextBox.Text;
            var lastName = registerView.LastNameTextBox.Text;
            var email = registerView.EmailTextBox.Text;
            var password = registerView.PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please complete all input fields!", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 6 characters long and contain at least one number and one uppercase letter!", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsUsernameTaken(userName))
            {
                MessageBox.Show("This username is already taken. Please choose a different one!", "Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsEmailTaken(email))
            {
                var result = MessageBox.Show("A player is already registered with this e-mail address. Please use a different e-mail address or select the forgot password option.Do you want to use the forgot password option?", "Registration", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    NavigateToLoginWindow(registerView);
                }
                return;
            }

            int tokens = 50; // Bonus tokens
            string hashedPassword = HashHelper.ComputeSha256Hash(password);
            DateTime timeOfRegistration = DateTime.Now;

            var dbManager = new DbManager();
            string query = "INSERT INTO USER (UserName, Password, FirstName, LastName, Email, TimeOfRegistration, Score, Tokens, Avatar) " +
                           "VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @TimeOfRegistration, 0, @Tokens, @Avatar)";
            var parameters = new Dictionary<string, object>
            {
                { "@UserName", userName },
                { "@Password", hashedPassword },
                { "@FirstName", firstName },
                { "@LastName", lastName },
                { "@Email", email },
                { "@TimeOfRegistration", timeOfRegistration },
                { "@Tokens", tokens },
                { "@Avatar", $"Avatar_{userName}.png" }
            };

            dbManager.ExecuteNonQuery(query, parameters);

            MessageBox.Show("Registration successful! You have received 50 bonus tokens.", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show("You will be directed to the Login Window where you can log in with your newly registered account.", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigateToLoginWindow(registerView);

        }

        public void NavigateToLoginWindow(RegisterView registerView)
        {
            var parentWindow = Window.GetWindow(registerView);
            if (parentWindow == null)
            {
                Debug.WriteLine("Parent window is null.");
                return;
            }

            // Find the named ContentControl
            var contentControl = parentWindow.FindName("MainContentControl") as ContentControl;
            if (contentControl == null)
            {
                Debug.WriteLine("ContentControl is null.");
                return;
            }

            // Swap to LoginView
            var loginView = new LoginView
            {
                DataContext = new LoginViewModel()
            };
            contentControl.Content = loginView;
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
        private bool IsUsernameTaken(string username)
        {
            var dbManager = new DbManager();
            string query = "SELECT COUNT(*) FROM USER WHERE UserName = @UserName";
            var parameters = new Dictionary<string, object>
            {
                { "@UserName", username }
            };
            var result = dbManager.ExecuteQuery(query, parameters);
            return Convert.ToInt32(result.Rows[0][0]) > 0;
        }
        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsDigit) &&
                   password.Any(char.IsUpper);
        }
    }
}
