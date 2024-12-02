using nonogram.MVVM.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for SearchBarView.xaml
    /// </summary>
    public partial class SearchBarView : UserControl
    {
        public SearchBarView()
        {
            InitializeComponent();
            //this.DataContext = this;
            // Get the MainViewModel from the MainWindow's DataContext
            var mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;

            if (mainViewModel != null)
            {
                DataContext = mainViewModel.SearchBarVM;
            }
            else
            {
                throw new InvalidOperationException("MainWindow's DataContext is not of type MainViewModel.");
            }

            Debug.WriteLine($"SearchBarView Initialized. DataContext: {DataContext?.GetType().Name ?? "null"}");
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is SearchBarViewModel viewModel && viewModel.SearchCommand.CanExecute(null))
                {
                    viewModel.SearchCommand.Execute(null);
                }
            }
        }


        //private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        var textBox = sender as TextBox;
        //        var searchTerm = textBox?.Text;

        //        Debug.WriteLine($"SearchBox_KeyDown called. SearchTerm: '{searchTerm}'");

        //        //// Manually call FilterImages (ensure you have access to the ImageListViewModel instance)
        //        //var mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;
        //        //mainViewModel?.ImageListVM.FilterImages(searchTerm);
        //    }
        //}


    }

}
