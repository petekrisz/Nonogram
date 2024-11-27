using System.Windows.Controls;
using System.Windows.Input;
using nonogram.MVVM.ViewModel;
using nonogram.MVVM.Model;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
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

        //private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Debug.WriteLine("MouseEnter event triggered");
        //    if (sender is Border border && border.DataContext is GridElement element)
        //    {

        //        element.IsHighlighted = true; // Trigger PropertyChanged

        //    }
        //}

        //private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Debug.WriteLine("MouseLeave event triggered");
        //    if (sender is Border border && border.DataContext is GridElement element)
        //    {

        //        element.IsHighlighted = false; // Reset highlight
        //    }
        //}
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.DataContext is GridElement element && DataContext is GameViewModel viewModel)
            {
                Debug.WriteLine($"MouseEnter: Highlighting Row: {element.Row}, Column: {element.Column}");

                // Highlight the entire row and column
                viewModel.HighlightRowAndColumn(element.Row, element.Column, true);
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.DataContext is GridElement element && DataContext is GameViewModel viewModel)
            {
                Debug.WriteLine($"MouseLeave: Un-highlighting Row: {element.Row}, Column: {element.Column}");

                // Un-highlight the entire row and column
                viewModel.HighlightRowAndColumn(element.Row, element.Column, false);
            }
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is GridElement element && DataContext is GameViewModel viewModel)
            {
                // Cycle through the click states
                element.ClickState = (element.ClickState + 1) % 4;

                // Ensure the TextBlock is accessed correctly
                var textBlock = border.Child as TextBlock;

                switch (element.ClickState)
                {
                    case 0: // Default state
                        border.Background = new SolidColorBrush(Color.FromRgb(255, 247, 204)); // #FFF7CC
                        if (textBlock != null)
                        {
                            textBlock.Visibility = Visibility.Hidden;
                            textBlock.Text = ""; // Clear text
                        }
                        break;

                    case 1: // Black background
                        border.Background = new SolidColorBrush(Colors.Black);
                        if (textBlock != null)
                        {
                            textBlock.Visibility = Visibility.Hidden;
                        }
                        break;

                    case 2: // Revert to original background (#FFF7CC), show "X"
                        border.Background = new SolidColorBrush(Color.FromRgb(255, 247, 204)); // #FFF7CC
                        if (textBlock != null)
                        {
                            textBlock.Visibility = Visibility.Visible;
                            textBlock.Text = "X";
                        }
                        break;

                    case 3: // Keep original background (#FFF7CC), show "?"
                        if (textBlock != null)
                        {
                            textBlock.Visibility = Visibility.Visible;
                            textBlock.Text = "?";
                        }
                        break;
                }

                // Debug output for tracing
                Debug.WriteLine($"Cell clicked at Row: {element.Row}, Column: {element.Column}, ClickState: {element.ClickState}");
            }
        }



    }



    //private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
    //{
    //    if (DataContext is GameViewModel viewModel)
    //    {
    //        if (e.Delta > 0)
    //        {
    //            viewModel.CellSize += 2;
    //            viewModel.FontSize += 1;
    //        }
    //        else if (e.Delta < 0)
    //        {
    //            viewModel.CellSize = Math.Max(10, viewModel.CellSize - 2);
    //            viewModel.FontSize = Math.Max(6, viewModel.FontSize - 1);
    //        }
    //    }
    //}

    //private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    //{
    //    Debug.WriteLine("MouseLeftButtonUp Event Triggered");
    //    if (sender is TextBlock textBlock && textBlock.DataContext is GameCell cell)
    //    {
    //        if (DataContext is GameViewModel viewModel)
    //        {
    //            viewModel.CellClicked(cell);
    //        }
    //    }
    //}
}

