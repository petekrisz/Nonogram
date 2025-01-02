using nonogram.MVVM.Model;
using nonogram.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserMenuView.xaml
    /// </summary>
    public partial class UserMenuView : UserControl
    {
        public UserMenuView()
        {
            InitializeComponent();
        }
        private void OnImageSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentPresenter contentPresenter && contentPresenter.Content is ListImage listImage && DataContext is UserMenuViewModel viewModel)
            {
                viewModel.ImageSelectedCommand.Execute(listImage);
            }
        }
    }
}
