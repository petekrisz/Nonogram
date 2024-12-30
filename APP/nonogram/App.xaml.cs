using nonogram.DB;
using nonogram.MVVM.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;

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


            // Show the login window
            var loginWindow = new LoginWindow();
            loginWindow.DataContext = new LoginViewModel();
            loginWindow.ShowDialog();

            // Retrieve the login view model
            var loginViewModel = loginWindow.DataContext as LoginViewModel;

            if (loginViewModel != null && loginViewModel.IsLoginSuccessful)
            {
                // Pass the username to the MainViewModel
                var mainWindow = new MainWindow()
                {
                    DataContext = new MainViewModel(loginViewModel.UserName)
                };
                Debug.WriteLine("MainWindow is about to be shown.");
                mainWindow.Show();
            }
        }
    }
}
