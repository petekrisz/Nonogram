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

                // Update the element's properties to reflect the new state
                switch (element.ClickState)
                {
                    case 0: // Default state
                        element.IsHighlighted = false; // Use IsHighlighted for default background
                        if (border.Child is TextBlock textBlock)
                        {
                            textBlock.Visibility = Visibility.Hidden;
                            textBlock.Text = viewModel.GameGrid.ImageCells[element.Row][element.Column].ToString();
                        }
                        break;

                    case 1: // Black background
                        element.IsHighlighted = false; // Ensure IsHighlighted doesn't affect this state
                        if (border.Child is TextBlock textBlockHidden)
                        {
                            textBlockHidden.Visibility = Visibility.Hidden;
                        }
                        break;

                    case 2: // Original background, show "X"
                        element.IsHighlighted = true; // Enable highlighting
                        if (border.Child is TextBlock textBlockX)
                        {
                            textBlockX.Visibility = Visibility.Visible;
                            textBlockX.Text = "X";
                        }
                        break;

                    case 3: // Original background, show "?"
                        element.IsHighlighted = true; // Enable highlighting
                        if (border.Child is TextBlock textBlockQuestion)
                        {
                            textBlockQuestion.Visibility = Visibility.Visible;
                            textBlockQuestion.Text = "?";
                        }
                        break;
                }

                // Debugging log
                Debug.WriteLine($"Cell clicked at Row: {element.Row}, Column: {element.Column}, ClickState: {element.ClickState}");

                // Reapply highlight if the mouse is still over the cell
                if (border.IsMouseOver)
                {
                    viewModel.HighlightRowAndColumn(element.Row, element.Column, true);
                }
            }
        }
    }
}