using nonogram.Common;
using nonogram.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace nonogram.MVVM.Model
{
    public class GameGrid : ObservableObject
    {
        public List<List<char>> ImageCells { get; set; }
        public List<List<int>> RowHints { get; private set; }
        public List<List<int>> ColumnHints { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int MaxRowHintCount { get; private set; }
        public int MaxColumnHintCount { get; private set; }

        public GameGrid(IMAGE image)
        {
            Rows = image.Rows;
            Columns = image.Columns;
            ImageCells = new List<List<char>>();
            RowHints = new List<List<int>>();
            ColumnHints = new List<List<int>>();
            MaxRowHintCount = 0;
            MaxColumnHintCount = 0;
            InitializeGrid(image.Content);
            CalculateHints();
        }




        private void InitializeGrid(string content)
        {
            for (int i = 0; i < Rows; i++)
            {
                var temp = content.Skip(i * Columns).Take(Columns).ToList();
                ImageCells.Add(temp);
            }
        }

        private void CalculateHints()
        {
            // Calculate RowHints
            RowHints = ImageCells
                .Select(row => CalculateConsecutiveCells(new string(row.ToArray())))
                .ToList();

            MaxRowHintCount = RowHints.Max(hint => hint.Count);

            // Calculate ColumnHints
            ColumnHints = Enumerable.Range(0, Columns)
                .Select(col => CalculateConsecutiveCells(new string(ImageCells.Select(row => row[col]).ToArray())))
                .ToList();

            MaxColumnHintCount = ColumnHints.Max(hint => hint.Count);
        }

        private List<int> CalculateConsecutiveCells(string line)
        {
            return line
                .Aggregate(new List<int> { 0 }, (acc, c) =>
                {
                    if (c == '1')
                        acc[acc.Count - 1]++;
                    else if (acc[acc.Count - 1] > 0)
                        acc.Add(0);
                    return acc;
                })
                .Where(count => count > 0)
                .ToList();
        }

        public List<List<int>> GetRowHints()
        {
            return RowHints.Select(list => Enumerable.Repeat(0, MaxRowHintCount - list.Count).Concat(list).ToList()).ToList();
        }

        public List<List<int>> GetHorizontalColumnHints()
        {
            List<List<int>> HorizontalColumnHints = new List<List<int>>();
            for (int i = 0; i < MaxColumnHintCount; i++)
            {
                HorizontalColumnHints.Add(new List<int>());
            }

            // Fill the transformed list
            for (int i = 0; i < MaxColumnHintCount; i++)
            {
                for (int j = 0; j < ColumnHints.Count; j++)
                {
                    int indexFromEnd = ColumnHints[j].Count - 1 - i;
                    if (indexFromEnd >= 0)
                    {
                        HorizontalColumnHints[i].Add(ColumnHints[j][indexFromEnd]);
                    }
                    else
                    {
                        HorizontalColumnHints[i].Add(0);
                    }
                }
            }

            // Reverse the order of the outer list to match the desired output
            HorizontalColumnHints.Reverse();

            return HorizontalColumnHints;
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