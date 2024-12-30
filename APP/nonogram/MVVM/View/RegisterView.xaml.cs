using nonogram.MVVM.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as RegisterViewModel;
            viewModel?.RegisterCommand.Execute(this);
        }
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is RegisterViewModel viewModel)
            {
                viewModel.NavigateToLoginCommand.Execute(null);
            }
        }
    }
}
