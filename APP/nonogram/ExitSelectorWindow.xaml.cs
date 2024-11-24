using System;
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
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();    // That leads back to the LoginWindow
            loginWindow.Show();
            Application.Current.MainWindow.Close();         // After opening the LoginWindow it closes this one (MainWindow)
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();                 // This closes the whole application
        }

        private void ExitSelectorWindow_Deactivated(object sender, EventArgs e)
        {
            this.Close();                                   // If clicked by mistake, clicking outside the window brings back the user to the app
        }

    }
}
