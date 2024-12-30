using nonogram.Common;
using nonogram.DB;
using System;
using System.IO;
using System.Windows;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for ExitSelectorWindow.xaml
    /// </summary>
    public partial class ExitSelectorWindow : Window
    {
        public ExitSelectorWindow()
        {
            InitializeComponent();

            Logout.Click += Logout_Click;
            Exit.Click += Exit_Click;

            this.Deactivated += ExitSelectorWindow_Deactivated;

            Application.Current.Exit += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            string sourceDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB");

            // Adjust the target directory to point to the correct nonogram.DB folder
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string targetDirectory = Path.Combine(projectDirectory, "DB");

            // Copy CSV files from source to target
            foreach (var filePath in Directory.GetFiles(sourceDirectory, "*.csv"))
            {
                string fileName = Path.GetFileName(filePath);
                string targetPath = Path.Combine(targetDirectory, fileName);

                try
                {
                    File.Copy(filePath, targetPath, overwrite: true);
                    Console.WriteLine($"Copied {fileName} to {targetPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying {fileName}: {ex.Message}");
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ExportAllTablesToCsv();                         // Export database tables to CSV files
            Application.Current.MainWindow.Close();         // Close the MainWindow
            this.Close();                                   // Close the ExitSelectorWindow


            var viewModelFactory = new ViewModelFactory();
            var loginWindow = new LoginWindow();
            var loginViewModel = viewModelFactory.CreateLoginViewModel();
            loginWindow.DataContext = loginViewModel;
            loginWindow.ShowDialog();

            if (loginViewModel != null && loginViewModel.IsLoginSuccessful)
            {
                var mainWindow = new MainWindow()
                {
                    DataContext = viewModelFactory.CreateMainViewModel(loginViewModel.UserName)
                };
                mainWindow.Show();
            }


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ExportAllTablesToCsv();                         // Export database tables to CSV files

            Application.Current.Shutdown();                 // This closes the whole application
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

        private void ExitSelectorWindow_Deactivated(object sender, EventArgs e)
        {
            this.Close();                                   // If clicked by mistake, clicking outside the window brings back the user to the app
        }

    }
}
