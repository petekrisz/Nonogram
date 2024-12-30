using System.Windows;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitSelectorWindow exitSelector = new ExitSelectorWindow
            {
                Owner = this // Set the owner to the main window
            };
            exitSelector.ShowDialog();
        }
    }
}
