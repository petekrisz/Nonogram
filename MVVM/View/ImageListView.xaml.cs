using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using nonogram.MVVM.Model;
using nonogram.MVVM.ViewModel;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for ImageListView.xaml
    /// </summary>
    public partial class ImageListView : UserControl
    {
        public ImageListView()
        {
            InitializeComponent();
            
        }

        private void OnImageSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentPresenter contentPresenter && contentPresenter.Content is ListImage listImage && DataContext is ImageListViewModel viewModel && Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.GameViewCommand.Execute(listImage.Image);
            }
        }

    }
}
