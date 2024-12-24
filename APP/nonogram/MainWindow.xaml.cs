using nonogram.MVVM.ViewModel;
using System.Windows;

namespace nonogram
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void Avatar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is MVVM.ViewModel.MainViewModel viewModel)
            {
                viewModel.OpenUserMenuView();
            }
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
