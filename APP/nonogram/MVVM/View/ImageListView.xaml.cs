using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using nonogram.DB;
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
            if (sender is ContentPresenter contentPresenter &&
                contentPresenter.Content is ListImage listImage &&
                DataContext is ImageListViewModel viewModel &&
                Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
            {
                // Fetch the IMAGE object using the IMAGEId
                DbManager dbManager = new DbManager();
                IMAGE selectedImage = dbManager.GetImageById(listImage.IMAGEId);

                if (selectedImage != null)
                {
                    // Pass the IMAGE object to the GameViewCommand
                    mainViewModel.GameViewCommand.Execute(selectedImage);
                }
            }
        }

    }
}
