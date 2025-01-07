using nonogram.DB;
using nonogram.MVVM.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using nonogram.Common;
using nonogram.MVVM.View;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            try
            {
                // Initialize the database
                var dbManager = new DbManager();
                dbManager.InitializeDatabaseAndTables();
                //Debug.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }

            var viewModelFactory = new ViewModelFactory();
            var loginViewModel = viewModelFactory.CreateLoginViewModel();
            var loginWindow = new LoginWindow();
            var loginView = new LoginView
            {
                DataContext = loginViewModel
            };
            loginWindow.MainContentControl.Content = loginView;
            loginWindow.Show();

            //Debug.WriteLine(loginViewModel == null ? "--loginViewModel is null" : "--loginViewModel is not null");
            //Debug.WriteLine($"App.xaml.cs: LoginViewModel DataContext: {loginViewModel.GetHashCode()}");

            loginViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(LoginViewModel.IsLoginSuccessful) && loginViewModel.IsLoginSuccessful)
                {
                    //Debug.WriteLine("LoginVM is not null & Login is successful.");
                    // Close the login window
                    loginWindow.Close();

                    // Pass the username to the MainViewModel
                    var mainWindow = new MainWindow
                    {
                        DataContext = viewModelFactory.CreateMainViewModel(loginViewModel.UserName)
                    };
                    //Debug.WriteLine("MainWindow is about to be shown.");
                    mainWindow.Show();
                }
            };
        }
    }
}
