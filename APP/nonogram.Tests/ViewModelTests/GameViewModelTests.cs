using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.MVVM.ViewModel;
using nonogram.DB;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace nonogram.Tests.ViewModelTests
{
    [TestClass]
    public class GameViewModelTests
    {
        private GameViewModel _viewModel;
        private IMAGE _testImage;
        private Window _mockWindow;

        [TestInitialize]
        public void SetUp()
        {
            // Create a test IMAGE object
            _testImage = new IMAGE
            {
                IMAGEId = 1,
                Title = "Test Image",
                IMAGERows = 5,
                IMAGEColumns = 5,
                Content = "1111100000111110000011111",
                RowFinished = "00000",
                ColumnFinished = "00000"
            };

            // Create a mock Window
            _mockWindow = new Window();

            // Initialize the GameViewModel with the test IMAGE object and mock Window
            _viewModel = new GameViewModel(_testImage, "testUser", _mockWindow);

            _viewModel.GuessGrid = new List<List<char>>
            {
                new List<char> { '?', '1', '1', '1', '1' },
                new List<char> { '?', 'x', 'x', 'x', 'x' },
                new List<char> { '1', 'x', 'x', 'x', 'x' },
                new List<char> { '0', 'x', 'x', 'x', 'x' },
                new List<char> { '1', 'x', 'x', 'x', 'x' }
            };

            //For test purpose ImageCellTableElements is initialized with relevant values values
            _viewModel.ImageCellTableElements = new ObservableCollection<GridElement>
            {
                new GridElement { Row = 0, Column = 0, ClickState = 1 },
                new GridElement { Row = 0, Column = 1, ClickState = 1 },
                new GridElement { Row = 0, Column = 2, ClickState = 1 },
                new GridElement { Row = 0, Column = 3, ClickState = 1 },
                new GridElement { Row = 0, Column = 4, ClickState = 1 },
                new GridElement { Row = 1, Column = 0, ClickState = 0 },
                new GridElement { Row = 2, Column = 0, ClickState = 1 },
                new GridElement { Row = 3, Column = 0, ClickState = 0 },
                new GridElement { Row = 4, Column = 0, ClickState = 1 }
            };
        }

        [TestMethod]
        public void GameViewModel_ShouldInitializeCorrectly()
        {
            // Assert
            Assert.AreEqual(_testImage.IMAGEId, _viewModel.ImageId, "ImageId should be set correctly.");
            Assert.AreEqual(_testImage.IMAGERows, _viewModel.GameGrid.Rows, "Rows should be set correctly.");
            Assert.AreEqual(_testImage.IMAGEColumns, _viewModel.GameGrid.Columns, "Columns should be set correctly.");
            CollectionAssert.AreEqual(_testImage.RowFinished.Select(c => int.Parse(c.ToString())).ToArray(), _viewModel.RowFinished, "RowFinished should be set correctly.");
            CollectionAssert.AreEqual(_testImage.ColumnFinished.Select(c => int.Parse(c.ToString())).ToArray(), _viewModel.ColumnFinished, "ColumnFinished should be set correctly.");
        }

        [TestMethod]
        public void CheckRowsAndColumns_ShouldUpdateGameStateCorrectly_NoAction()
        {
            // Arrange
            int row = 0;
            int column = 0;
            int clickState = 0;

            // Act
            _viewModel.CheckRowsAndColumns(row, column, clickState);

            // Assert
            // Verify that the game state is updated correctly
            Assert.AreEqual('x', _viewModel.GuessGrid[row][column], "GuessGrid should be updated correctly.");
            Assert.AreEqual(0, _viewModel.RowFinished[row], "RowFinished should be updated correctly.");
            Assert.AreEqual(0, _viewModel.ColumnFinished[column], "ColumnFinished should be updated correctly.");
        }

        [TestMethod]
        public void CheckRowsAndColumns_ShouldUpdateGameStateCorrectly_CorrectFill()
        {
            // Arrange
            int row = 0;
            int column = 0;
            int clickState = 1;

            // Act
            _viewModel.CheckRowsAndColumns(row, column, clickState);

            // Assert
            // Verify that the game state is updated correctly
            Assert.AreEqual('1', _viewModel.GuessGrid[row][column], "GuessGrid should be updated correctly.");
            Assert.AreEqual(1, _viewModel.RowFinished[row], "RowFinished should be updated correctly.");
            Assert.AreEqual(1, _viewModel.ColumnFinished[column], "ColumnFinished should be updated correctly.");
        }

        [TestMethod]
        public void GameWon_ShouldTriggerCorrectly()
        {
            // Arrange
            bool eventTriggered = false;
            _viewModel.GameWon += (sender, e) => eventTriggered = true;

            // Act
            // Simulate winning the game
            for (int i = 0; i < _viewModel.GameGrid.Rows; i++)
            {
                for (int j = 0; j < _viewModel.GameGrid.Columns; j++)
                {
                    _viewModel.GuessGrid[i][j] = '1'; // Assume '1' represents a filled cell
                }
            }
            _viewModel.CheckIfGameIsWon();

            // Assert
            Assert.IsTrue(eventTriggered, "GameWon event should be triggered when the game is won.");
        }
    }
}
