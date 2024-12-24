using System.Windows;
using System.Windows.Controls;

namespace nonogram.MVVM.View
{
    public partial class UserMenuView : UserControl
    {
        public UserMenuView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel.UserMenuViewModel viewModel)
            {
                viewModel.NewPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
