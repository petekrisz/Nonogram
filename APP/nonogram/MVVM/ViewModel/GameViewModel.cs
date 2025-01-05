using nonogram.MVVM.Model;
using nonogram.MVVM.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using nonogram.DB;
using nonogram.Common;
using System.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using System.Windows.Shell;
using static System.Net.Mime.MediaTypeNames;

namespace nonogram.MVVM.ViewModel
{
    public class GameViewModel : ObservableObject
    {
        public event EventHandler GameWon;
        private readonly string _username;
        public GameGrid GameGrid { get; private set; }
        public int ImageId { get; private set; }

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

        public int[] RowFinished { get; private set; }
        public int[] ColumnFinished { get; private set; }
        public List<List<char>> GuessGrid { get; private set; }
        public List<List<char>> TempList { get; private set; }


        public GameViewModel(IMAGE selectedImage, string username)
        {
            _username = username;
            GameGrid = new GameGrid(selectedImage);
            ImageId = selectedImage.IMAGEId; // Store IMAGEId
            Debug.WriteLine($"GameViewModel initialized with IMAGEId: {ImageId}");

            // Initialize collections
            ColumnTableElements = new ObservableCollection<GridElement>();
            RowTableElements = new ObservableCollection<GridElement>();
            ImageCellTableElements = new ObservableCollection<GridElement>();

            // Initialize RowFinished, ColumnFinished, and GuessGrid
            RowFinished = GameGrid.RowFinished.Select(c => int.Parse(c.ToString())).ToArray();
            ColumnFinished = GameGrid.ColumnFinished.Select(c => int.Parse(c.ToString())).ToArray();
            // Debugging: Print the arrays to verify initialization
            Debug.WriteLine("RowFinished: " + string.Join(",", RowFinished));
            Debug.WriteLine("ColumnFinished: " + string.Join(",", ColumnFinished));


            GuessGrid = new List<List<char>>();
            TempList = new List<List<char>>();
            for (int i = 0; i < GameGrid.Rows; i++)
            {
                GuessGrid.Add(Enumerable.Repeat('x', GameGrid.Columns).ToList());
                TempList.Add(Enumerable.Repeat('*', GameGrid.Columns).ToList());
            }

            // Draw the tables
            DrawTable(GameGrid.MaxColumnHintCount, GameGrid.Columns, "HorizontalColumnHints");
            DrawTable(GameGrid.Rows, GameGrid.MaxRowHintCount, "RowHints");
            DrawTable(GameGrid.Rows, GameGrid.Columns, "ImageCells");

            CheckUnfinishedImage();



        }

        private void CheckUnfinishedImage()
        {
            DbManager dbManager = new DbManager();
            string query = @"
                            SELECT Content
                            FROM USERIMAGE
                            WHERE UserName = @UserName AND IMAGEId = @IMAGEId";
            var parameters = new Dictionary<string, object>
            {
                { "@UserName", _username },
                { "@IMAGEId", ImageId }
            };
            var dataTable = dbManager.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                string content = dataTable.Rows[0]["Content"].ToString();
                if (content.Length > 1)
                {
                    var guessGrid = new List<List<char>>();
                    for (int i = 0; i < GameGrid.Rows; i++)
                    {
                        var row = content.Substring(i * GameGrid.Columns, GameGrid.Columns).ToList();
                        guessGrid.Add(row);
                    }

                    for (int r = 0; r < GameGrid.Rows; r++)
                    {
                        for (int c = 0; c < GameGrid.Columns; c++)
                        {
                            GuessGrid[r][c] = guessGrid[r][c];
                            var element = ImageCellTableElements.FirstOrDefault(e => e.Row == r && e.Column == c);
                            if (element != null)
                            {
                                switch (guessGrid[r][c])
                                {
                                    case 'x':
                                        element.ClickState = 0;
                                        break;
                                    case '1':
                                        element.ClickState = 1;
                                        break;
                                    case '0':
                                        element.ClickState = 2;
                                        break;
                                    case '?':
                                        element.ClickState = 3;
                                        break;
                                }
                            }
                        }
                    }

                    Debug.WriteLine($"Unfinished image with content: {content}");
                    for (int r = 0; r < GameGrid.Rows; r++)
                    {
                        CheckRow(r);
                    }
                    for (int c = 0; c < GameGrid.Columns; c++)
                    {
                        CheckColumn(c);
                    }
                }
            }
            else
            {
                // Apply background and click state changes if necessary in the case of non-started, virgin image
                if (RowFinished.Any(r => r == 1) || ColumnFinished.Any(c => c == 1))
                {
                    ApplyBackgroundAndClickState();
                }
            }
        }

        public void SaveGameState()
        {
            string content = string.Join("", GuessGrid.SelectMany(row => row));
            DbManager dbManager = new DbManager();
            string query = @"
                            INSERT INTO USERIMAGE (UserName, IMAGEId, Finished, Content)
                            VALUES (@UserName, @IMAGEId, false, @Content)
                            ON DUPLICATE KEY UPDATE Finished = false, Content = @Content";
            var parameters = new Dictionary<string, object>
            {
                { "@UserName", _username },
                { "@IMAGEId", ImageId },
                { "@Content", content }
            };
            dbManager.ExecuteNonQuery(query, parameters);
            Debug.WriteLine($"Game state saved for IMAGEId: {ImageId} with content: {content}");
        }

        public void CheckRowsAndColumns(int row, int column, int clickState)
        {
            // Register the click state into the GuessGrid
            switch (clickState)
            {
                case 0:
                    GuessGrid[row][column] = 'x';
                    break;
                case 1:
                    GuessGrid[row][column] = '1';
                    break;
                case 2:
                    GuessGrid[row][column] = '0';
                    break;
                case 3:
                    GuessGrid[row][column] = '?';
                    break;
            }

            // Check the row
            CheckRow(row);

            // Check the column
            CheckColumn(column);

            // Check if the game is won
            if (RowFinished.All(r => r == 1))
            {
                CheckIfGameIsWon();
            }
        }

        private void CheckColumn(int column)
        {
            string guessColumnString = new string(GuessGrid.Select(r => r[column]).ToArray());
            //Debug.WriteLine($"GuessColumnString: {guessColumnString}");
            //Debug.WriteLine(GameGrid.ColumnHints[column].ToArray().ToString());

            string columnString = new string(guessColumnString.Select(c => c == 'x' || c == '?' ? '0' : c).ToArray());
            //Debug.WriteLine($"ColumnString: {columnString}");

            List<int> calculateGuess = GameGrid.CalculateConsecutiveCells(columnString);
            if (calculateGuess.SequenceEqual(GameGrid.ColumnHints[column]))
            {
                foreach (var element in ColumnTableElements.Where(e => e.Column == column))
                {
                    element.InitialBackground = Brushes.LightSteelBlue;
                }

                if (ColumnFinished[column] != 1)
                {

                    // Update non-`1` cells in the column to `ClickState 2` (X)
                    foreach (var element in ImageCellTableElements.Where(e => e.Column == column && e.ClickState != 1))
                    {
                        element.ClickState = 2; // Set the ClickState to 2 (X)
                        GuessGrid[element.Row][element.Column] = '0'; // Reflect in GuessGrid
                    }

                    ColumnFinished[column] = 1;
                    //Debug.WriteLine($"ColumnFinished {column} 1");
                }
            }
            else if (!CheckString(guessColumnString, GameGrid.ColumnHints[column]))
            {
                ColumnFinished[column] = 0;
                //Debug.WriteLine($"ColumnFinished {column} 0");
                foreach (var element in ColumnTableElements.Where(e => e.Column == column))
                {
                    element.InitialBackground = Brushes.LightCoral;
                }
            }
            else
            {
                ColumnFinished[column] = 0;
                //Debug.WriteLine($"ColumnFinished {column} 0");
                foreach (var element in ColumnTableElements.Where(e => e.Column == column))
                {
                    element.InitialBackground = Brushes.LightGray;
                }
            }
        }

        private void CheckRow(int row)
        {
            string guessRowString = new string(GuessGrid[row].ToArray());
            Debug.WriteLine($"GuessRowString: {guessRowString}");
            Debug.WriteLine(GameGrid.RowHints[row].ToArray().ToString());

            string rowString = new string(guessRowString.Select(c => c == 'x' || c == '?' ? '0' : c).ToArray());
            //Debug.WriteLine($"RowString: {rowString}");

            List<int> calculateGuess = GameGrid.CalculateConsecutiveCells(rowString);


            if (calculateGuess.SequenceEqual(GameGrid.RowHints[row]))
            {
                foreach (var element in RowTableElements.Where(e => e.Row == row))
                {
                    element.InitialBackground = Brushes.LightSteelBlue;
                }

                if (RowFinished[row] != 1)
                {
                    // Update non-`1` cells in the row to `ClickState 2` (x)
                    foreach (var element in ImageCellTableElements.Where(e => e.Row == row && e.ClickState != 1))
                    {
                        element.ClickState = 2; // 1
                        GuessGrid[element.Row][element.Column] = '0'; // Reflect in GuessGrid
                    }

                    RowFinished[row] = 1;
                    //Debug.WriteLine($"Row {row} finished");
                }


            }
            else if (!CheckString(guessRowString, GameGrid.RowHints[row]))
            {
                RowFinished[row] = 0;
                //Debug.WriteLine($"RowFinished {row} 0");
                foreach (var element in RowTableElements.Where(e => e.Row == row))
                {
                    element.InitialBackground = Brushes.LightCoral;
                }
            }
            else
            {
                RowFinished[row] = 0;
                //Debug.WriteLine($"RowFinished {row} 0");
                foreach (var element in RowTableElements.Where(e => e.Row == row))
                {
                    element.InitialBackground = Brushes.LightGray;
                }
            }
        }

        private void CheckIfGameIsWon()
        {
            bool gridsMatch = true;
            for (int i = 0; i < GameGrid.Rows; i++)
            {
                for (int j = 0; j < GameGrid.Columns; j++)
                {
                    //Debug.WriteLine($"GuessGrid[{i}][{j}]: {GuessGrid[i][j]}, GameGrid.ImageCells[{i}][{j}]: {GameGrid.ImageCells[i][j]}");
                    if (GuessGrid[i][j] != '1' && GameGrid.ImageCells[i][j] == '1')
                    {
                        gridsMatch = false;
                        break;
                    }
                }
                if (!gridsMatch) break;
            }

            if (gridsMatch)
            {
                GameWon?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool CheckString(string input, List<int> hints)
        {
            Debug.WriteLine($"CheckString called. Input: {input}, Substrings: {string.Join(", ", hints)}");

            List<string> patterns = hints.Select(n => new string('1', n)).ToList();
            input = input.Replace('x', '?');

            int totalOnes = 0;
            foreach (var pattern in patterns)
            {
                totalOnes += pattern.Length;
            }

            int availableOnes = 0;
            foreach (var c in input)
            {
                if (c == '?' || c == '1') availableOnes++;
            }

            Debug.WriteLine($"Total required 1's: {totalOnes}, Available 1's (including ?): {availableOnes}");
            if (availableOnes < totalOnes) return false;
            return CanPlacePatterns(input.ToCharArray(), patterns, 0, 0, totalOnes);
        }

        private bool CanPlacePatterns(char[] input, List<string> patterns, int patternIndex, int startIndex, int remainingOnes)
        {
            if (patternIndex >= patterns.Count)
            {
                int currentOnes = 0;
                foreach (var c in input)
                {
                    if (c == '1') currentOnes++;
                }
                Debug.WriteLine($"Final check: Current 1's in string: {currentOnes}, Remaining 1's: {remainingOnes}");
                return currentOnes == remainingOnes;
            }

            string currentPattern = patterns[patternIndex];
            for (int i = startIndex; i <= input.Length - currentPattern.Length; i++)
            {
                if (CanPlacePatternAt(input, currentPattern, i))
                {
                    char[] originalState = (char[])input.Clone();
                    Debug.WriteLine($"Trying to place pattern '{currentPattern}' at position {i}: {new string(input)}");

                    PlacePattern(input, currentPattern, i);
                    Debug.WriteLine($"Placed pattern '{currentPattern}' at position {i}: {new string(input)}");

                    if (CanPlacePatterns(input, patterns, patternIndex + 1, i + currentPattern.Length + 1, remainingOnes))
                        return true;
                    Array.Copy(originalState, input, input.Length);
                    Debug.WriteLine($"Reverted to previous state after trying '{currentPattern}' at position {i}: {new string(input)}");
                }
            }
            return false;
        }

        private bool CanPlacePatternAt(char[] input, string pattern, int position)
        {
            for (int j = 0; j < pattern.Length; j++)
            {
                if (input[position + j] == '0') return false;
            }
            return true;
        }

        private void PlacePattern(char[] input, string pattern, int position)
        {
            for (int j = 0; j < pattern.Length; j++)
            {
                input[position + j] = '1';
            }
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
            //Debug.WriteLine($"HighlightRowAndColumn called. Row: {row}, Column: {column}, Highlight: {highlight}");

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
                    //Debug.WriteLine($"Element Row: {cellGridElement.Row}, Column: {cellGridElement.Column}, ClickState: {cellGridElement.ClickState}, Highlight: {highlight}");

                    // Ensure cells with ClickState 1 are not highlighted
                    if (cellGridElement.ClickState != 1)
                    {
                        cellGridElement.IsHighlighted = highlight;
                    }
                }
            }
        }

        private void ApplyBackgroundAndClickState()
        {
            // Check rows
            for (int row = 0; row < RowFinished.Length; row++)
            {
                if (RowFinished[row] == 1)
                {
                    var rowElements = ImageCellTableElements.Where(e => e.Row == row).ToList();

                    if (rowElements.All(e => e.Element is TextBlock textBlock && textBlock.Text == "0"))
                    {
                        foreach (var element in rowElements)
                        {
                            element.ClickState = 2;
                            GuessGrid[row][element.Column] = '0'; // Update GuessGrid
                        }
                    }
                    else if (rowElements.All(e => e.Element is TextBlock textBlock && textBlock.Text == "1"))
                    {
                        foreach (var element in rowElements)
                        {
                            element.ClickState = 1;
                            GuessGrid[row][element.Column] = '1'; // Update GuessGrid
                        }
                    }
                    else
                    {
                        foreach (var element in rowElements)
                        {
                            if (element.Element is TextBlock textBlock)
                            {
                                if (textBlock.Text == "1")
                                {
                                    element.ClickState = 1;
                                    GuessGrid[row][element.Column] = '1';
                                }
                                else
                                {
                                    element.ClickState = 2;
                                    GuessGrid[row][element.Column] = '0';
                                }
                            }
                        }
                    }
                    foreach (var element in RowTableElements.Where(e => e.Row == row))
                    {
                        element.InitialBackground = Brushes.LightSteelBlue;
                    }
                }
            }

            // Check columns
            for (int col = 0; col < ColumnFinished.Length; col++)
            {
                if (ColumnFinished[col] == 1)
                {
                    var colElements = ImageCellTableElements.Where(e => e.Column == col).ToList();

                    if (colElements.All(e => e.Element is TextBlock textBlock && textBlock.Text == "0"))
                    {
                        foreach (var element in colElements)
                        {
                            element.ClickState = 2;
                            GuessGrid[element.Row][col] = '0'; // Update GuessGrid
                        }
                    }
                    else if (colElements.All(e => e.Element is TextBlock textBlock && textBlock.Text == "1"))
                    {
                        foreach (var element in colElements)
                        {
                            element.ClickState = 1;
                            GuessGrid[element.Row][col] = '1'; // Update GuessGrid
                        }
                    }
                    else
                    {
                        foreach (var element in colElements)
                        {
                            if (element.Element is TextBlock textBlock)
                            {
                                if (textBlock.Text == "1")
                                {
                                    element.ClickState = 1;
                                    GuessGrid[element.Row][col] = '1';
                                }
                                else
                                {
                                    element.ClickState = 2;
                                    GuessGrid[element.Row][col] = '0';
                                }
                            }
                        }
                    }
                    foreach (var element in ColumnTableElements.Where(e => e.Column == col))
                    {
                        element.InitialBackground = Brushes.LightSteelBlue;
                    }
                }
            }
        }

        public bool ExecuteHelpOption(int row, int column, string typeOfHelp)
        {
            Debug.WriteLine($"ExecuteHelpOption is called: {typeOfHelp} at Row: {row}, Column: {column}");

            bool success = false;
            int r, c;
            List<int> rowList = new List<int>();
            List<int> columnList = new List<int>();

            // Implement the logic for each help option
            switch (typeOfHelp)
            {
                case "H1":
                    // Logic for H1 help option
                    Debug.WriteLine($"Executing H1 help option at Row: {row}, Column: {column}");
                    RevealCell(row, column);
                    CheckRow(row);
                    CheckColumn(column);

                    success = true;
                    break;
                case "H3":
                    // Logic for H3 help option
                    Debug.WriteLine($"Executing H3 help option at Row: {row}, Column: {column}");
                    UpdateTempList();
                    CorrectGuessesOrRevealNew(typeOfHelp, 3, TempList);

                    success = true;
                    break;
                case "H8":
                    // Logic for H8 help option
                    Debug.WriteLine($"Executing H8 help option at Row: {row}, Column: {column}");
                    UpdateTempList();
                    CorrectGuessesOrRevealNew(typeOfHelp, 8, TempList);

                    success = true;
                    break;
                case "H13":
                    // Logic for H13 help option
                    Debug.WriteLine($"Executing H13 help option at Row: {row}, Column: {column}");
                    List<List<int>> diamond = new List<List<int>>
                    {
                        new List<int> { row - 2, column },
                        new List<int> { row - 1, column - 1 },
                        new List<int> { row - 1, column },
                        new List<int> { row - 1, column + 1 },
                        new List<int> { row, column - 2 },
                        new List<int> { row, column - 1 },
                        new List<int> { row, column },
                        new List<int> { row, column + 1 },
                        new List<int> { row, column + 2 },
                        new List<int> { row + 1, column - 1 },
                        new List<int> { row + 1, column },
                        new List<int> { row + 1, column + 1 },
                        new List<int> { row + 2, column }
                    };

                    foreach (var item in diamond)
                    {
                        row = item[0];
                        column = item[1];
                        RevealCell(row, column);

                    }

                    rowList = diamond.Select(pair => pair[0]).Where(x => x >= 0 && x < GameGrid.Rows).Distinct().ToList();
                    columnList = diamond.Select(pair => pair[1]).Where(x => x >= 0 && x < GameGrid.Columns).Distinct().ToList();
                    CheckHelpRowsAndColumns(rowList, columnList);

                    success = true;
                    break;
                case "L1":
                    // Logic for L1 help option
                    Debug.WriteLine($"Executing L1 help option at Row: {row}, Column: {column}");
                    if (column == -1)
                    {
                        for (c = 0; c < GameGrid.Columns; c++)
                        {
                            RevealCell(row, c);
                        }
                        CheckHelpRowsAndColumns(null, Enumerable.Repeat(0, GameGrid.Columns).ToList());
                    }
                    else if (row == -1)
                    {
                        for (r = 0; r < GameGrid.Columns; r++)
                        {
                            RevealCell(r, column);
                        }
                        CheckHelpRowsAndColumns(Enumerable.Repeat(0, GameGrid.Rows).ToList(), null);
                    }

                    success = true;
                    break;
                case "L3":
                    // Logic for L3 help option
                    Debug.WriteLine($"Executing L3 help option at Row: {row}, Column: {column}");

                    if (column == -1)
                    {
                        for(r = row-1;r<=row+1; r++) 
                        {
                            for (c = 0; c < GameGrid.Columns; c++)
                            {
                                RevealCell(r, c);
                            }
                        }
                        CheckHelpRowsAndColumns(null, Enumerable.Repeat(0, GameGrid.Columns).ToList());
                    }
                    else if (row == -1)
                    {
                        for (c = column - 1; c <= column + 1; c++)
                        {
                            for (r = 0; r < GameGrid.Columns; r++)
                            {
                                RevealCell(r, c);
                            }
                        }
                        CheckHelpRowsAndColumns(Enumerable.Repeat(0, GameGrid.Rows).ToList(), null);
                    }

                    success = true;
                    break;
                case "Check3H":
                    // Logic for Check3H help option
                    Debug.WriteLine($"Executing Check3H help option at Row: {row}, Column: {column}");
                    UpdateTempList();
                    if (!TempList.Any(x => x.Contains('-')))
                    {
                        MessageBox.Show("No wrong guessing! No help will be charged!", "Good Work", MessageBoxButton.OK);
                        success = false;
                    }
                    else
                    {
                        int v = TempList.SelectMany(innerList => innerList).Count(n => n == '-');
                        v = v > 3 ? 3 : v;
                        CorrectGuessesOrRevealNew(typeOfHelp, v, TempList);
                        success = true;
                    }

                    break;
                case "Erase":
                    // Logic for Erase help option
                    Debug.WriteLine($"Executing Erase help option at Row: {row}, Column: {column}");
                    UpdateTempList();
                    for (int i = 0; i < TempList.Count; i++)
                    {
                        for (int j = 0; j < TempList[i].Count; j++)
                        {
                            if (TempList[i][j] == '-')
                            {
                                GuessGrid[i][j] = 'x';
                                var element = ImageCellTableElements.FirstOrDefault(e => e.Row == i && e.Column == j);
                                if (element != null)
                                {
                                    element.ClickState = 0;
                                }
                                rowList.Add(i);
                                columnList.Add(j);
                            }
                        }
                    }
                    CheckHelpRowsAndColumns(rowList.Distinct().ToList(), columnList.Distinct().ToList());

                    success = true;
                    break;
                default:
                    Debug.WriteLine($"Unknown help option: {typeOfHelp}");
                    success = true;
                    break;
            }

            CheckIfGameIsWon();
            Debug.WriteLine($"ExecuteHelpOption success: {success}");
            return success;
        }

        private void CorrectGuessesOrRevealNew(string typeOfHelp, int v, List<List<char>> tempList)
        {
            Random random = new Random();
            int final = 0;

            while (final < v)
            {
                int r = random.Next(GameGrid.Rows);
                int c = random.Next(GameGrid.Columns);

                if (typeOfHelp == "Check3H" && tempList[r][c] == '-')
                {
                    GuessGrid[r][c] = GameGrid.ImageCells[r][c];
                    var element = ImageCellTableElements.FirstOrDefault(e => e.Row == r && e.Column == c);
                    if (element != null)
                    {
                        element.ClickState = (GameGrid.ImageCells[r][c] == '1') ? 1 : 2;
                    }
                    CheckRow(r);
                    CheckColumn(c);
                    final++;
                }
                else if (typeOfHelp != "Check3H" && (tempList[r][c] == '-' || tempList[r][c] == '*'))
                {
                    GuessGrid[r][c] = GameGrid.ImageCells[r][c];
                    var element = ImageCellTableElements.FirstOrDefault(e => e.Row == r && e.Column == c);
                    if (element != null)
                    {
                        element.ClickState = (GameGrid.ImageCells[r][c] == '1') ? 1 : 2;
                    }
                    CheckRow(r);
                    CheckColumn(c);
                    final++;
                }
            }
        }

        private void RevealCell(int r, int c)
        {
            Debug.WriteLine($"RevealCell called. Row: {r}, Column: {c}");
            if (r >= 0 && c >= 0 && r < GameGrid.Rows && c < GameGrid.Columns)
            {
                char value = GameGrid.ImageCells[r][c];
                GuessGrid[r][c] = value;

                var element = ImageCellTableElements.FirstOrDefault(e => e.Row == r && e.Column == c);
                if (element != null)
                {
                    if (value == '1')
                    {
                        element.ClickState = 1;
                    }
                    else if (value == '0')
                    {
                        element.ClickState = 2;
                    }
                }
            }
        }
        private void CheckHelpRowsAndColumns(List<int> rowsToCheck, List<int> columnsToCheck)
        {
            // Check rows
            if (rowsToCheck != null)
            {
                foreach (int row in rowsToCheck)
                {
                    CheckRow(row);
                }
            }

            // Check columns
            if (columnsToCheck != null)
            {
                foreach (int column in columnsToCheck)
                {
                    CheckColumn(column);
                }
            }
        }

        public void UpdateTempList()
        {
            for (int i = 0; i < GuessGrid.Count; i++)
            {
                for (int j = 0; j < GuessGrid[i].Count; j++)
                {
                    if (GuessGrid[i][j] == '0' || GuessGrid[i][j] == '1')
                    {
                        if (GuessGrid[i][j] == GameGrid.ImageCells[i][j])
                        {
                            TempList[i][j] = '+';
                        }
                        else
                        {
                            TempList[i][j] = '-';
                        }
                    }
                    else
                    {
                        TempList[i][j] = '*';
                    }
                }
            }
        }
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
                    //Debug.WriteLine($"IsHighlighted updated to: {_isHighlighted} for Row: {Row}, Column: {Column}");
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}