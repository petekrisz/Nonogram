using nonogram.DB;
using nonogram.MVVM.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using nonogram.Common;

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
                Console.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }

            var viewModelFactory = new ViewModelFactory();
            var loginWindow = new LoginWindow();
            var loginViewModel = viewModelFactory.CreateLoginViewModel();
            loginWindow.DataContext = loginViewModel;
            loginWindow.ShowDialog();

            Debug.WriteLine(loginViewModel == null ? "--loginViewModel is null" : "--loginViewModel is not null");
            Debug.WriteLine($"App.xaml.cs: LoginViewModel DataContext: {loginViewModel.GetHashCode()}");

            if (loginViewModel != null)
            {
                Debug.WriteLine("LoginViewModel is not null.");
                Debug.WriteLine($"IsLoginSuccessful: {loginViewModel.IsLoginSuccessful}");
                if (loginViewModel.IsLoginSuccessful)
                {
                    Debug.WriteLine("LoginVM in not null & Login is successful.");
                    // Pass the username to the MainViewModel
                    var mainWindow = new MainWindow()
                    {
                        DataContext = viewModelFactory.CreateMainViewModel(loginViewModel.UserName)
                    };
                    Debug.WriteLine("MainWindow is about to be shown.");
                    mainWindow.Show();
                }
            }
            else
            {
                Debug.WriteLine("LoginViewModel is null.");
            }
        }
    }
}
