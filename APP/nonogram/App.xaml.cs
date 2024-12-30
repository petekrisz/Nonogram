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


            var loginWindow = new LoginWindow
            {
                DataContext = new LoginViewModel() // Assign the initial ViewModel
            };

            if (loginWindow.ShowDialog() == true) // ShowDialog returns DialogResult
            {
                var loginViewModel = loginWindow.DataContext as LoginViewModel;
                if (loginViewModel != null && loginViewModel.IsLoginSuccessful)
                {
                    Debug.WriteLine("Login successful. Instantiating MainWindow...");

                    var mainWindow = new MainWindow
                    {
                        DataContext = new MainViewModel(loginViewModel.UserName)
                    };

                    mainWindow.Show(); // Display MainWindow
                }
            }
            else
            {
                Debug.WriteLine("Login was cancelled or failed.");
                Shutdown(); // Exit the application if login fails
            }
        }
    }
}
