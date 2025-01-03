using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ToolTip avatarToolTip;

        public MainWindow()
        {
            InitializeComponent();
            avatarToolTip = new ToolTip
            {
                Content = "Click on the avatar image to access the user menu.",
                Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse
            };
        }

        private void Avatar_MouseEnter(object sender, MouseEventArgs e)
        {
            avatarToolTip.IsOpen = true;
        }

        private void Avatar_MouseLeave(object sender, MouseEventArgs e)
        {
            avatarToolTip.IsOpen = false;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitSelectorWindow exitSelector = new ExitSelectorWindow
            {
                Owner = this // Set the owner to the main window
            };
            exitSelector.ShowDialog();
        }

        private void Avatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MVVM.ViewModel.MainViewModel viewModel)
            {
                viewModel.UserMenuViewCommand.Execute(null);
            }
        }
    }
}
