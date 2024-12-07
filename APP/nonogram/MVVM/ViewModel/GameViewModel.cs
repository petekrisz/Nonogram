﻿using nonogram.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using nonogram.DB;
using nonogram.Common;
using System.Linq;
using System;
using System.Data.Common;
using System.ComponentModel;
using System.Diagnostics;

namespace nonogram.MVVM.ViewModel
{
    public class GameViewModel : ObservableObject
    {
        public GameGrid GameGrid { get; private set; }


        private ObservableCollection<GridElement> _columnTableElements;
        public ObservableCollection<GridElement> ColumnTableElements
        {
            get => _columnTableElements;
            set
            {
                _columnTableElements = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GridElement> _rowTableElements;
        public ObservableCollection<GridElement> RowTableElements
        {
            get => _rowTableElements;
            set
            {
                _rowTableElements = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<GridElement> _imageCellTableElements;
        public ObservableCollection<GridElement> ImageCellTableElements
        {
            get => _imageCellTableElements;
            set
            {
                _imageCellTableElements = value;
                OnPropertyChanged();
            }
        }

        public GameViewModel(IMAGE selectedImage)
        {
            GameGrid = new GameGrid(selectedImage);

            // Initialize collections
            ColumnTableElements = new ObservableCollection<GridElement>();
            RowTableElements = new ObservableCollection<GridElement>();
            ImageCellTableElements = new ObservableCollection<GridElement>();

            // Draw the tables
            DrawTable(GameGrid.MaxColumnHintCount, GameGrid.Columns, "HorizontalColumnHints");
            DrawTable(GameGrid.Rows, GameGrid.MaxRowHintCount, "RowHints");
            DrawTable(GameGrid.Rows, GameGrid.Columns, "ImageCells");
        }

        public void DrawTable(int rows, int columns, string listName)
        {
            ObservableCollection<GridElement> targetCollection = null;
            List<List<object>> dataList = null;

            // Determine the target collection and data source based on the table type
            if (listName == "HorizontalColumnHints")
            {
                dataList = GameGrid.GetHorizontalColumnHints().Select(r => r.Cast<object>().ToList()).ToList();
                targetCollection = ColumnTableElements;
            }
            else if (listName == "RowHints")
            {
                dataList = GameGrid.GetRowHints().Select(r => r.Cast<object>().ToList()).ToList();
                targetCollection = RowTableElements;
            }
            else if (listName == "ImageCells")
            {
                dataList = GameGrid.ImageCells.Select(r => r.Cast<object>().ToList()).ToList();
                targetCollection = ImageCellTableElements;
            }

            if (dataList != null && targetCollection != null)
            {
                targetCollection.Clear();

                // Populate the collection with GridElements
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        var cellValue = dataList[i][j]; //Get the value (either char or int)

                        // Determine the initial background based on the table type
                        var initialBrush = new SolidColorBrush(Color.FromRgb(255, 247, 204)); // Default for ImageCells
                        if (listName == "RowHints" || listName == "HorizontalColumnHints")
                        {
                            initialBrush = Brushes.LightGray; // Example color for hints
                        }


                        // Create a new GridElement
                        var gridElement = new GridElement
                        {
                            Row = i,
                            Column = j,
                            InitialBackground = initialBrush, // Set the initial background
                            IsHighlighted = false, // Default highlighting state
                            ClickState = 0, // Default click state
                        };

                        // Create a TextBlock for the GridElement
                        var textBlock = new TextBlock
                        {
                            Text = cellValue.ToString(),
                            FontSize = 10,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };


                        gridElement.Element = textBlock;

                        // Add the GridElement to the collection
                        targetCollection.Add(gridElement);
                    }
                }
            }
        }

        public void HighlightRowAndColumn(int row, int column, bool highlight)
        {
            Debug.WriteLine($"HighlightRowAndColumn called. Row: {row}, Column: {column}, Highlight: {highlight}");

            // Highlight corresponding row in RowTableElements            
            foreach (var rowElement in RowTableElements.Where(r => r.Row == row))
            {
                if (rowElement is GridElement rowGridElement)
                {
                    rowGridElement.IsHighlighted = highlight;
                }
            }

            // Highlight corresponding column in ColumnTableElements
            foreach (var columnElement in ColumnTableElements.Where(c => c.Column == column))
            {
                if (columnElement is GridElement columnGridElement)
                {
                    columnGridElement.IsHighlighted = highlight;
                }
            }

            // Highlight the corresponding cell in the ImageCellTableElements
            foreach (var cellElement in ImageCellTableElements.Where(c => c.Row == row || c.Column == column))
            {
                if (cellElement is GridElement cellGridElement)
                {
                    Debug.WriteLine($"Element Row: {cellGridElement.Row}, Column: {cellGridElement.Column}, ClickState: {cellGridElement.ClickState}, Highlight: {highlight}");

                    // Ensure cells with ClickState 1 are not highlighted
                    if (cellGridElement.ClickState != 1)
                    {
                        cellGridElement.IsHighlighted = highlight;
                    }
                }
            }
        }



        //public void HighlightRowAndColumn(int row, int column, bool highlight)
        //{
        //    Debug.WriteLine($"HighlightRowAndColumn called. Row: {row}, Column: {column}, Highlight: {highlight}");

        //    // For debugging each element being updated
        //    foreach (var element in ImageCellTableElements.Where(e => e.Row == row || e.Column == column))
        //    {
        //        Debug.WriteLine($"Element to be highlighted: Row {element.Row}, Column {element.Column}");
        //    }

        //    // Highlight all elements in the row
        //    foreach (var element in ImageCellTableElements.Where(e => e.Row == row))
        //    {
        //        element.IsHighlighted = highlight;
        //    }

        //    // Highlight all elements in the column
        //    foreach (var element in ImageCellTableElements.Where(e => e.Column == column))
        //    {
        //        element.IsHighlighted = highlight;
        //    }

        //    // Optionally, handle RowTableElements and ColumnTableElements
        //    foreach (var rowElement in RowTableElements.Where(e => e.Row == row))
        //    {
        //        rowElement.IsHighlighted = highlight;
        //    }

        //    foreach (var columnElement in ColumnTableElements.Where(e => e.Column == column))
        //    {
        //        columnElement.IsHighlighted = highlight;
        //    }
        //}


        //public void HighlightRowAndColumn(int row, int column, bool highlight)
        //{
        //    // Update RowTableElements
        //    foreach (var rowElement in RowTableElements.Where(r => r.Row == row))
        //    {
        //        var textBlock = rowElement.Element as TextBlock;
        //        if (textBlock != null)
        //        {
        //            textBlock.Background = highlight ? new SolidColorBrush(Colors.WhiteSmoke) : new SolidColorBrush(Colors.Transparent);
        //        }
        //    }

        //    // Update ColumnTableElements
        //    foreach (var columnElement in ColumnTableElements.Where(c => c.Column == column))
        //    {
        //        var textBlock = columnElement.Element as TextBlock;
        //        if (textBlock != null)
        //        {
        //            textBlock.Background = highlight ? new SolidColorBrush(Colors.WhiteSmoke) : new SolidColorBrush(Colors.Transparent);
        //        }
        //    }

        //    // Update ImageCellTableElements
        //    foreach (var cell in ImageCellTableElements.Where(c => c.Row == row && c.Column == column))
        //    {
        //        var textBlock = cell.Element as TextBlock;
        //        if (textBlock != null)
        //        {
        //            textBlock.Background = highlight ? new SolidColorBrush(Colors.WhiteSmoke) : new SolidColorBrush(Colors.Transparent);
        //        }
        //    }
        //}


    }

    public class GridElement : INotifyPropertyChanged
    {
        private int _clickState;
        private bool _isHighlighted;
        private Brush _initialBackground;

        public UIElement Element { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Brush InitialBackground
        {
            get => _initialBackground;
            set
            {
                if (_initialBackground != value)
                {
                    _initialBackground = value;
                    OnPropertyChanged(nameof(InitialBackground));
                }
            }
        }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    Debug.WriteLine($"IsHighlighted updated to: {_isHighlighted} for Row: {Row}, Column: {Column}");
                    OnPropertyChanged(nameof(IsHighlighted));
                }
            }
        }
        public int ClickState
        {
            get => _clickState;
            set
            {
                if (_clickState != value)
                {
                    _clickState = value;
                    OnPropertyChanged(nameof(ClickState));
                }
            }
        }

        private Visibility _visibility = Visibility.Hidden;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        //private Brush _background = new SolidColorBrush(Color.FromRgb(255, 247, 204));
        //public Brush Background
        //{
        //    get => _background;
        //    set
        //    {
        //        _background = value;
        //        OnPropertyChanged(nameof(Background));
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}