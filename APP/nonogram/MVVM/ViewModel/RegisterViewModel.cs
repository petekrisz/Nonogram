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
    public class RegisterViewModel : ObservableObject
    {
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private readonly LoginViewModel _loginViewModel;

        public RegisterViewModel(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
            RegisterCommand = new RelayCommand<object>(Register);
            NavigateToLoginCommand = new RelayCommand<object>(parameter => NavigateToLoginWindow(_loginViewModel));
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
                    NavigateToLoginWindow(_loginViewModel);
                }
                return;
            }

            int tokens = 50; // Bonus tokens
            string hashedPassword = HashHelper.ComputeSha256Hash(password);
            DateTime timeOfRegistration = DateTime.Now;

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

            var loginViewModel = new LoginViewModel();
            NavigateToLoginWindow(_loginViewModel);

        }

        public void NavigateToLoginWindow(LoginViewModel loginViewModel)
        {
            var parentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
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
                DataContext = loginViewModel
            };
            Debug.WriteLine($"NavigateToLoginWindow: LoginView DataContext: {loginView.DataContext.GetHashCode()}");
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
