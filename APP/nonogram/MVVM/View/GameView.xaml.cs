using System.Windows.Controls;
using System.Windows.Input;
using nonogram.MVVM.ViewModel;
using nonogram.MVVM.Model;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;
using System.Linq;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public string SelectedHelpOption { get; set; }

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(GameView), new PropertyMetadata(1.0));

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }
        public GameView()
        {
            InitializeComponent();

            // Check if the control is loaded and populated
            Debug.WriteLine("GameView loaded and DataContext set.");

        }
        public void SetViewModel(GameViewModel viewModel)
        {
            DataContext = viewModel;
        }
        private void GameView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Determine the zoom direction (up or down)
            if (e.Delta > 0) // Scroll up (zoom in)
            {
                ZoomLevel += 0.1; // Increase scale (zoom in)
            }
            else if (e.Delta < 0) // Scroll down (zoom out)
            {
                ZoomLevel -= 0.1; // Decrease scale (zoom out)
            }

            // Set a limit to prevent the zoom from becoming too small or too large
            ZoomLevel = Math.Max(0.1, Math.Min(ZoomLevel, 3.0)); // Min 0.1x, Max 3.0x zoom

            // Apply the scaling to the ZoomContainer
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            // Apply the ScaleTransform to the ZoomContainer
            var scaleTransform = new ScaleTransform(ZoomLevel, ZoomLevel);
            ZoomContainer.LayoutTransform = scaleTransform;
        }
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.DataContext is GridElement element && DataContext is GameViewModel viewModel)
            {
                //Debug.WriteLine($"MouseEnter: Highlighting Row: {element.Row}, Column: {element.Column}");

                // Highlight the entire row and column
                viewModel.HighlightRowAndColumn(element.Row, element.Column, true);
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.DataContext is GridElement element && DataContext is GameViewModel viewModel)
            {
                //Debug.WriteLine($"MouseLeave: Un-highlighting Row: {element.Row}, Column: {element.Column}");

                // Un-highlight the entire row and column
                viewModel.HighlightRowAndColumn(element.Row, element.Column, false);
            }
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Cell_MouseLeftButtonDown");
            Debug.WriteLine($"SelectedHelpOption: {SelectedHelpOption}");
            if (sender is Border border && border.DataContext is GridElement element && DataContext is GameViewModel viewModel)
            {
                if (!string.IsNullOrEmpty(SelectedHelpOption))
                {
                    Debug.WriteLine($"SelectedHelpOption: {SelectedHelpOption}");
                    bool helpExecuted = false;

                    if ((SelectedHelpOption == "H1" || SelectedHelpOption == "H13") && IsChildOf(border, ImageCellItemsControl))
                    {
                        // Call the method in GameViewModel with the coordinates of a GameGrid cell
                        helpExecuted = viewModel.ExecuteHelpOption(element.Row, element.Column, SelectedHelpOption);
                    }
                    else if (SelectedHelpOption == "L1" || SelectedHelpOption == "L3" )
                    {
                        // Call the method in GameViewModel with the coordinates of a RowTableElements of ColumnTableElements cell
                        if (IsChildOf(border, RowItemsControl))
                        {
                            Debug.WriteLine($"RowTableElement clicked at Row: {element.Row}");
                            helpExecuted = viewModel.ExecuteHelpOption(element.Row, -1, SelectedHelpOption);
                        }
                        else if (IsChildOf(border, ColumnItemsControl))
                        {
                            Debug.WriteLine($"ColumnTableElement clicked at Column: {element.Column}");
                            helpExecuted = viewModel.ExecuteHelpOption(-1, element.Column, SelectedHelpOption);
                        }
                    }

                    if (helpExecuted)
                    {
                        // Call a method in HelpTableViewModel to decrease the value
                        if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.DataContext is MainViewModel mainViewModel)
                        {
                            Debug.WriteLine($"---> HelpTableVM accessed in GameView: {mainViewModel.HelpTableVM.GetHashCode()}");
                            mainViewModel.HelpTableVM.DecreaseHelpOptionValue(SelectedHelpOption);
                        }
                    }

                    // Reset the SelectedHelpOption after use
                    SelectedHelpOption = null;
                }
                else if (IsChildOf(border, ImageCellItemsControl))
                {
                    // Cycle through the click states
                    element.ClickState = (element.ClickState + 1) % 4;

                    // Call CheckRowsAndColumns method
                    viewModel.CheckRowsAndColumns(element.Row, element.Column, element.ClickState);

                    // Debugging log
                    Debug.WriteLine($"Cell clicked at Row: {element.Row}, Column: {element.Column}, ClickState: {element.ClickState}");
                }


                // Reapply highlight if the mouse is still over the cell
                if (border.IsMouseOver)
                {
                    viewModel.HighlightRowAndColumn(element.Row, element.Column, true);
                }
            }
        }

        private bool IsChildOf(DependencyObject child, DependencyObject parent)
        {
            while (child != null)
            {
                if (child == parent)
                    return true;
                child = VisualTreeHelper.GetParent(child);
            }
            return false;
        }
    }
}