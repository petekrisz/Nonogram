using nonogram.MVVM.ViewModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;
                if (mainViewModel == null)
                {
                    throw new InvalidOperationException("MainWindow's DataContext is invalid.");
                }
                DataContext = mainViewModel.SearchBarVM;
                Debug.WriteLine($"SearchBarView DataContext: {DataContext?.GetType().Name ?? "null"}");
            }
            else
            {
                DataContext = new SearchBarViewModel(); // Provide dummy data for design-time
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
    }
}
