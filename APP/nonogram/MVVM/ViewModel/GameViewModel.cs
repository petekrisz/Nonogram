using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.Model;
using System.Diagnostics;

namespace nonogram.MVVM.ViewModel
{
    public class GameViewModel : ObservableObject
    {
        private double _cellSize = 18;
        public double CellSize
        {
            get => _cellSize;
            set
            {
                _cellSize = value;
                OnPropertyChanged();
            }
        }

        private double _fontSize = 10;
        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        public GameGrid GameGrid { get; set; }

        public GameViewModel(IMAGE image)
        {
            GameGrid = new GameGrid(image);
            Debug.WriteLine($"ColumnNumbersTable count: {GameGrid.ColumnNumbersTable?.Count}");
            Debug.WriteLine($"RowNumbersTable count: {GameGrid.RowNumbersTable?.Count}");
        }

        public void CellClicked(GameCell cell)
        {
            switch (cell.State)
            {
                case 0:
                    cell.Background = "Black";
                    cell.DisplayText = string.Empty;
                    cell.State = 1;
                    break;
                case 1:
                    cell.Background = "#FFF7CC";
                    cell.DisplayText = "X";
                    cell.State = 2;
                    break;
                case 2:
                    cell.DisplayText = "?";
                    cell.State = 3;
                    break;
                case 3:
                    cell.DisplayText = string.Empty;
                    cell.State = 0;
                    break;
            }
        }
    }

}
