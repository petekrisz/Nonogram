using nonogram.DB;
using System;
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

        }





        /* This will open up the login window first....
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }*/



    }
}
