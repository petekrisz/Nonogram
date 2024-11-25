using nonogram.Common;
using nonogram.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace nonogram.MVVM.Model
{
    public class GameGrid : ObservableObject
    {
        public ObservableCollection<GameCell> Cells { get; set; }
        public ObservableCollection<ObservableCollection<int?>> RowNumbersTable { get; set; }
        public ObservableCollection<ObservableCollection<int?>> ColumnNumbersTable { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int RowCount => RowNumbersTable?.Count ?? 0;
        public int ColumnCount => ColumnNumbersTable?.Count ?? 0;


        public GameGrid(IMAGE image)
        {
            Rows = image.Rows;
            Columns = image.Columns;
            Cells = new ObservableCollection<GameCell>();
            RowNumbersTable = new ObservableCollection<ObservableCollection<int?>>();
            ColumnNumbersTable = new ObservableCollection<ObservableCollection<int?>>();

            CalculateNumbers(image);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cells.Add(new GameCell
                    {
                        Content = image.Content[i * Columns + j].ToString()
                    });
                }
            }
        }

        private List<int> CalculateConsecutiveCells(string line)
        {
            List<int> result = new List<int>();
            int count = 0;
            foreach (char c in line)
            {
                if (c == '1')
                {
                    count++;
                }
                else if (count > 0)
                {
                    result.Add(count);
                    count = 0;
                }
            }
            if (count > 0)
            {
                result.Add(count);
            }
            return result;
        }

        private void CalculateNumbers(IMAGE image)
        {
            // Calculate row numbers
            int columnRowNumbers = 0;
            List<List<int>> rowNumbers = new List<List<int>>();
            for (int i = 0; i < Rows; i++)
            {
                string row = image.Content.Substring(i * Columns, Columns);
                var rowNumbersList = CalculateConsecutiveCells(row);
                rowNumbers.Add(rowNumbersList);
                if (rowNumbersList.Count > columnRowNumbers)
                {
                    columnRowNumbers = rowNumbersList.Count;
                }
            }

            // Fill row numbers table
            for (int i = 0; i < Rows; i++)
            {
                var row = new ObservableCollection<int?>();
                for (int j = 0; j < columnRowNumbers; j++)
                {
                    if (j < rowNumbers[i].Count)
                    {
                        row.Insert(0, rowNumbers[i][rowNumbers[i].Count - 1 - j]);
                    }
                    else
                    {
                        row.Insert(0, null);
                    }
                }
                RowNumbersTable.Add(row);
            }

            // Calculate column numbers
            int rowColumnNumbers = 0;
            List<List<int>> columnNumbers = new List<List<int>>();
            for (int j = 0; j < Columns; j++)
            {
                var column = new System.Text.StringBuilder();
                for (int i = 0; i < Rows; i++)
                {
                    column.Append(image.Content[i * Columns + j]);
                }
                var columnNumbersList = CalculateConsecutiveCells(column.ToString());
                columnNumbers.Add(columnNumbersList);
                if (columnNumbersList.Count > rowColumnNumbers)
                {
                    rowColumnNumbers = columnNumbersList.Count;
                }
            }

            // Fill column numbers table
            for (int j = 0; j < Columns; j++)
            {
                var column = new ObservableCollection<int?>();
                for (int i = 0; i < rowColumnNumbers; i++)
                {
                    if (i < columnNumbers[j].Count)
                    {
                        column.Insert(0, columnNumbers[j][columnNumbers[j].Count - 1 - i]);
                    }
                    else
                    {
                        column.Insert(0, null);
                    }
                }
                ColumnNumbersTable.Add(column);
            }
        }
    }

    public class GameCell : ObservableObject
    {
        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        private string _displayText;
        public string DisplayText
        {
            get => _displayText;
            set
            {
                _displayText = value;
                OnPropertyChanged();
            }
        }

        private string _background;
        public string Background
        {
            get => _background;
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        private int _state;
        public int State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public GameCell()
        {
            Background = "#FFF7CC"; // Original background color
            DisplayText = string.Empty;
            State = 0; // Initial state
        }
    }

}
