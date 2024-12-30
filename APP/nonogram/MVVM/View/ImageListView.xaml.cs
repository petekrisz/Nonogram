using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
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
            Loaded += (s, e) =>
            {
                Debug.WriteLine($"ImageListView DataContext: {DataContext?.GetType().Name}");
                Debug.WriteLine($"---> ImageListView.DataContext: {DataContext?.GetHashCode()}");
            };
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

                    // Check if the image is already finished
                    string query = @"
                    SELECT Finished
                    FROM USERIMAGE
                    WHERE UserName = @UserName AND IMAGEId = @IMAGEId";
                    var parameters = new Dictionary<string, object>
                    {
                        { "@UserName", viewModel.Username },
                        { "@IMAGEId", selectedImage.IMAGEId }
                    };
                    var dataTable = dbManager.ExecuteQuery(query, parameters);

                    if (dataTable.Rows.Count > 0 && Convert.ToBoolean(dataTable.Rows[0]["Finished"]))
                    {
                        // Show message box with OK and Cancel buttons
                        MessageBoxResult result = MessageBox.Show(
                            "You have already solved the image. If you continue, the picture will be removed from your solved picture list.",
                            "Information",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Information);

                        if (result == MessageBoxResult.Cancel)
                        {
                            // Cancel the click
                            return;
                        }
                    }



                    // Pass the IMAGE object to the GameViewCommand
                    mainViewModel.GameViewCommand.Execute(selectedImage);
                }
            }
        }



    }
}
