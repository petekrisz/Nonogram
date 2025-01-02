using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.Model;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class UserMenuViewModel: ObservableObject
    {
        public string UserName { get; set; }

        private USER _user;
        public USER User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ObservableCollection<ListImage> UnfinishedImages { get; set; }

        public ICommand ChangeUsernameCommand { get; }
        public ICommand ChangeFirstNameCommand { get; }
        public ICommand ChangeLastNameCommand { get; }
        public ICommand ChangeEmailCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand ImageSelectedCommand { get; }

        public UserMenuViewModel(string userName)
        {
            UserName = userName;

            ChangeUsernameCommand = new RelayCommand<object>(ChangeUsername);
            ChangeEmailCommand = new RelayCommand<object>(ChangeEmail);
            ChangeFirstNameCommand = new RelayCommand<object>(ChangeFirstName);
            ChangeLastNameCommand = new RelayCommand<object>(ChangeLastName);
            ChangePasswordCommand = new RelayCommand<object>(ChangePassword);
            DeleteAccountCommand = new RelayCommand<object>(DeleteAccount);
            ImageSelectedCommand = new RelayCommand<ListImage>(OnImageSelected);

            // Load user details
            LoadUserData(UserName);
            // Load unfinished images
            LoadUnfinishedImages(UserName);
        }

        private void LoadUnfinishedImages(string userName)
        {
            var dbManager = new DbManager();
            string query = @"
                SELECT IMAGE.IMAGEId, IMAGE.Title, IMAGE.IMAGERows, IMAGE.IMAGEColumns, IMAGE.Score, IMAGE.CategoryLogo, IMAGE.ColourType
                FROM IMAGE
                JOIN USERIMAGE ON IMAGE.IMAGEId = USERIMAGE.IMAGEId
                WHERE USERIMAGE.UserName = @UserName AND USERIMAGE.Finished = false";
            var parameters = new Dictionary<string, object> { { "@UserName", userName } };
            var dataTable = dbManager.ExecuteQuery(query, parameters);

            UnfinishedImages = new ObservableCollection<ListImage>(
                dataTable.Rows.Cast<DataRow>().Select(row => new ListImage
                {
                    IMAGEId = Convert.ToInt32(row["IMAGEId"]),
                    ImageTitle = row["Title"].ToString(),
                    ImageSource = $"/Images/{row["CategoryLogo"].ToString().Replace("gold", "light")}",
                    ImageDetails = $"Colour: {(Convert.ToInt32(row["ColourType"]) == 0 ? "BW" : "C")} / Size: {row["IMAGERows"]} * {row["IMAGEColumns"]} / Score: {row["Score"]}"
                }).ToList());
        }

        private void OnImageSelected(ListImage selectedImage)
        {
            if (Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
            {
                var dbManager = new DbManager();
                var image = dbManager.GetImageById(selectedImage.IMAGEId);
                mainViewModel.GameViewCommand.Execute(image);
            }
        }


        private void DeleteAccount(object parameter)
        {
            var result = MessageBox.Show("Are you sure you want to delete your account? This action cannot be undone.", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var dbManager = new DbManager();
                try
                {
                    // Disable foreign key checks
                    dbManager.ExecuteNonQuery("SET FOREIGN_KEY_CHECKS=0;");

                    // Delete user data
                    var deleteUserQuery = "DELETE FROM USER WHERE UserName = @UserName;";
                    var deleteUserHelpQuery = "DELETE FROM USERHELP WHERE UserName = @UserName;";
                    var deleteUserImageQuery = "DELETE FROM USERIMAGE WHERE UserName = @UserName;";
                    var parameters = new Dictionary<string, object> { { "@UserName", UserName } };

                    dbManager.ExecuteNonQuery(deleteUserQuery, parameters);
                    dbManager.ExecuteNonQuery(deleteUserHelpQuery, parameters);
                    dbManager.ExecuteNonQuery(deleteUserImageQuery, parameters);

                    // Enable foreign key checks
                    dbManager.ExecuteNonQuery("SET FOREIGN_KEY_CHECKS=1;");

                    // Export tables to CSV
                    ExportAllTablesToCsv();

                    // Close the MainWindow and ExitSelectorWindow
                    Application.Current.MainWindow.Close();
                    (parameter as Window)?.Close();

                    // Show the LoginWindow
                    var viewModelFactory = new ViewModelFactory();
                    var loginWindow = new LoginWindow();
                    var loginViewModel = viewModelFactory.CreateLoginViewModel();
                    loginWindow.DataContext = loginViewModel;
                    loginWindow.ShowDialog();

                    if (loginViewModel != null && loginViewModel.IsLoginSuccessful)
                    {
                        var mainWindow = new MainWindow
                        {
                            DataContext = viewModelFactory.CreateMainViewModel(loginViewModel.UserName)
                        };
                        mainWindow.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the account: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangeUsername(object parameter)
        {
            var userMenuView = parameter as UserMenuView;
            var newUsername = userMenuView.newUsername.Text;

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("Please enter a username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UsernameValidator.IsUsernameTaken(newUsername))
            {
                MessageBox.Show("This username is already taken. Please choose a different one!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var dbManager = new DbManager();
                dbManager.ExecuteNonQuery("SET FOREIGN_KEY_CHECKS=0;");

                UpdateUserNameInDB(newUsername);

                dbManager.ExecuteNonQuery("SET FOREIGN_KEY_CHECKS=1;");

                UserName = newUsername;

                // Refresh user data in MainViewModel and UserMenuViewModel
                if (Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
                {
                    mainViewModel.LoadUserData(newUsername);
                    mainViewModel.UserMenuVM.LoadUserData(newUsername);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while changing the username: {ex.Message}");
            }
        }


        public void LoadUserData(string userName)
        {
            var dbManager = new DbManager();
            string query = "SELECT * FROM USER WHERE UserName = @UserName";
            var parameters = new Dictionary<string, object> { { "@UserName", userName } };
            var userTable = dbManager.ExecuteQuery(query, parameters);

            if (userTable.Rows.Count > 0)
            {
                var userRow = userTable.Rows[0];
                User = new USER
                {
                    UserName = userRow["UserName"].ToString(),
                    Password = userRow["Password"].ToString(),
                    FirstName = userRow["FirstName"].ToString(),
                    LastName = userRow["LastName"].ToString(),
                    Email = userRow["Email"].ToString()
                };
            }
        }

        public void ChangeEmail(object parameter)
        {
            var userMenuView = parameter as UserMenuView;
            var newEmail = userMenuView.newEmail.Text;
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("Please enter an e-mail address.");
                return;
            }
            if (!EmailValidator.IsValidEmail(newEmail))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }
            UpdateUserData("Email", newEmail);
            LoadUserData(UserName);

        }

        public void ChangeFirstName(object parameter)
        {
            var userMenuView = parameter as UserMenuView;
            var newFirstName = userMenuView.newFirstName.Text;
            if (string.IsNullOrWhiteSpace(newFirstName))
            {
                MessageBox.Show("Please enter first name.");
                return;
            }
            UpdateUserData("FirstName", newFirstName);
            LoadUserData(UserName);

        }

        public void ChangeLastName(object parameter)
        {
            var userMenuView = parameter as UserMenuView;
            var newLastName = userMenuView.newLastName.Text;
            if (string.IsNullOrWhiteSpace(newLastName))
            {
                MessageBox.Show("Please enter last name.");
                return;
            }
            UpdateUserData("LastName", newLastName);
            LoadUserData(UserName);

        }

        public void ChangePassword(object parameter)
        {
            var userMenuView = parameter as UserMenuView;
            var newPassword_1 = userMenuView.newPassword_1.Password;
            var newPassword_2 = userMenuView.newPassword_2.Password;

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
            string hashedPassword = HashHelper.ComputeSha256Hash(newPassword_1);

            UpdateUserData("Password", hashedPassword);
            LoadUserData(UserName);

        }

        private void UpdateUserData(string C, string newData)
        {
            var dbManager = new DbManager();
            string updateUser = $"UPDATE USER SET {C} = @NewData WHERE UserName = @UserName";
            var parameters = new Dictionary<string, object>
            {
                { "@NewData", newData },
                { "@UserName", UserName }
            };
            dbManager.ExecuteNonQuery(updateUser, parameters);
        }


        private void UpdateUserNameInDB(string newUsername)
        {
            var dbManager = new DbManager();
            var parameters = new Dictionary<string, object>
            {
                { "@NewUsername", newUsername },
                { "@OldUsername", UserName }
            };

            dbManager.ExecuteNonQuery("UPDATE USER SET UserName = @NewUsername WHERE UserName = @OldUsername;", parameters);
            dbManager.ExecuteNonQuery("UPDATE USERHELP SET UserName = @NewUsername WHERE UserName = @OldUsername;", parameters);
            dbManager.ExecuteNonQuery("UPDATE USERIMAGE SET UserName = @NewUsername WHERE UserName = @OldUsername;", parameters);
        }

        private void ExportAllTablesToCsv()
        {
            var dbManager = new DbManager();
            string basePath = "DB/";
            dbManager.ExportTableToCsv("USER", Path.Combine(basePath, "USER.csv"));
            dbManager.ExportTableToCsv("IMAGE", Path.Combine(basePath, "IMAGE.csv"));
            dbManager.ExportTableToCsv("HELP", Path.Combine(basePath, "HELP.csv"));
            dbManager.ExportTableToCsv("USERHELP", Path.Combine(basePath, "USERHELP.csv"));
            dbManager.ExportTableToCsv("USERIMAGE", Path.Combine(basePath, "USERIMAGE.csv"));
        }
    }
    public static class EmailValidator
    {
        private static readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
