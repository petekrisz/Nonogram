using System.Windows.Controls;
using System.Windows.Input;
using nonogram.MVVM.ViewModel;
using nonogram.MVVM.Model;
using System;
using System.Diagnostics;

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
        }

        public void SetViewModel(GameViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (DataContext is GameViewModel viewModel)
            {
                if (e.Delta > 0)
                {
                    viewModel.CellSize += 2;
                    viewModel.FontSize += 1;
                }
                else if (e.Delta < 0)
                {
                    viewModel.CellSize = Math.Max(10, viewModel.CellSize - 2);
                    viewModel.FontSize = Math.Max(6, viewModel.FontSize - 1);
                }
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("MouseLeftButtonUp Event Triggered");
            if (sender is TextBlock textBlock && textBlock.DataContext is GameCell cell)
            {
                if (DataContext is GameViewModel viewModel)
                {
                    viewModel.CellClicked(cell);
                }
            }
        }
    }
}
