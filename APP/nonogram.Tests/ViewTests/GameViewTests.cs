using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.MVVM.View;
using nonogram.MVVM.ViewModel;
using nonogram.DB;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using nonogram.Common;
using System.Windows.Media;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace nonogram.Tests.ViewTests
{
    [TestClass]
    public class GameViewTests
    {
        private GameView _gameView;
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

            // Initialize the GameView with the GameViewModel
            _gameView = new GameView();
            _gameView.DataContext = _viewModel;

            // Initialize ImageCellItemsControl and add it to the visual tree
            _gameView.ImageCellItemsControl = new ItemsControl();
            var grid = new Grid();
            grid.Children.Add(_gameView.ImageCellItemsControl);
            _mockWindow.Content = grid;
            _mockWindow.Show();

            // Create mock ImageCellTableElements
            _viewModel.ImageCellTableElements = new ObservableCollection<nonogram.MVVM.ViewModel.GridElement>
            {
                new nonogram.MVVM.ViewModel.GridElement { Row = 0, Column = 0, ClickState = 0 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 0, Column = 1, ClickState = 1 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 0, Column = 2, ClickState = 1 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 0, Column = 3, ClickState = 1 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 0, Column = 4, ClickState = 1 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 1, Column = 0, ClickState = 0 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 2, Column = 0, ClickState = 1 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 3, Column = 0, ClickState = 0 },
                new nonogram.MVVM.ViewModel.GridElement { Row = 4, Column = 0, ClickState = 1 }
            };

            // Add mock items to ImageCellItemsControl
            foreach (var element in _viewModel.ImageCellTableElements)
            {
                var border = new Border
                {
                    DataContext = element
                };
                border.MouseLeftButtonDown += _gameView.Cell_MouseLeftButtonDown;
                _gameView.ImageCellItemsControl.Items.Add(border);
            }

            // Initialize GuessGrid
            _viewModel.GuessGrid = new List<List<char>>();
            for (int i = 0; i < _testImage.IMAGERows; i++)
            {
                _viewModel.GuessGrid.Add(new List<char>(new char[_testImage.IMAGEColumns]));
                for (int j = 0; j < _testImage.IMAGEColumns; j++)
                {
                    _viewModel.GuessGrid[i][j] = 'x'; // Initialize with default value
                }
            }
        }

        //Test for the method GameView_ShouldInitializeCorrectly
        [TestMethod]
        public void GameView_ShouldInitializeCorrectly()
        {
            // Assert
            Assert.IsNotNull(_gameView.DataContext, "DataContext should be set.");
            Assert.AreEqual(_viewModel, _gameView.DataContext, "DataContext should be set to the GameViewModel.");
        }

        //Test for the method Cell_MouseLeftButtonDown
        [TestMethod]
        public void GameView_ShouldHandleCellClick()
        {
            // Arrange
            var cell = (Border)_gameView.ImageCellItemsControl.Items[0];

            // Print the visual tree for debugging
            Debug.WriteLine("Printing visual tree:");
            PrintVisualTree(_gameView);

            // Act
            cell.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent
            });

            // Assert
            Assert.AreEqual('1', _viewModel.GuessGrid[0][0], "GuessGrid should be updated correctly on cell click.");
        }

        //Tests for the method GameView_Zoom
        [TestMethod]
        public void ZoomIn_IncreasesZoomLevel()
        {
            // Arrange
            _gameView.ZoomLevel = 1.0;
            var args = new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, 120); // Scroll up
            args.RoutedEvent = UIElement.PreviewMouseWheelEvent;

            // Act
            _gameView.RaiseEvent(args);

            // Assert
            Assert.AreEqual(1.1, _gameView.ZoomLevel, 0.01);
        }

        [TestMethod]
        public void ZoomOut_DecreasesZoomLevel()
        {
            // Arrange
            _gameView.ZoomLevel = 1.0;
            var args = new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, -120); // Scroll down
            args.RoutedEvent = UIElement.PreviewMouseWheelEvent;

            // Act
            _gameView.RaiseEvent(args);

            // Assert
            Assert.AreEqual(0.9, _gameView.ZoomLevel, 0.01);
        }

        [TestMethod]
        public void Zoom_DoesNotExceedMaxZoomLevel()
        {
            // Arrange
            _gameView.ZoomLevel = 3.0;
            var args = new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, 120); // Scroll up
            args.RoutedEvent = UIElement.PreviewMouseWheelEvent;

            // Act
            _gameView.RaiseEvent(args);

            // Assert
            Assert.AreEqual(3.0, _gameView.ZoomLevel);
        }

        [TestMethod]
        public void Zoom_DoesNotGoBelowMinZoomLevel()
        {
            // Arrange
            _gameView.ZoomLevel = 0.1;
            var args = new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, -120); // Scroll down
            args.RoutedEvent = UIElement.PreviewMouseWheelEvent;

            // Act
            _gameView.RaiseEvent(args);

            // Assert
            Assert.AreEqual(0.1, _gameView.ZoomLevel);
        }

        //Tests for the method IsChildOf
        [TestMethod]
        public void IsChildOf_ReturnsTrue_WhenChildIsDirectDescendant()
        {
            // Arrange
            var parent = new Border();
            var child = new Border();

            // Add the child to the parent
            parent.Child = child;

            // Act
            var result = _gameView.IsChildOf(child, parent);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsChildOf_ReturnsFalse_WhenChildIsNotDescendant()
        {
            // Arrange
            var parent = new Border();
            var unrelated = new Border();

            // Act
            var result = _gameView.IsChildOf(unrelated, parent);

            // Assert
            Assert.IsFalse(result);
        }

        private void AddVisualChild(DependencyObject parent, DependencyObject child)
        {
            if (parent is UIElement parentElement && child is UIElement childElement)
            {
                var children = new VisualCollection(parentElement);
                children.Add(childElement);
            }
        }

        private void PrintVisualTree(DependencyObject parent, int level = 0)
        {
            Debug.WriteLine("Visual Tree:");
            if (parent == null) return;

            string indent = new string(' ', level * 2);
            Debug.WriteLine($"{indent}{parent.GetType().Name}");

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                PrintVisualTree(VisualTreeHelper.GetChild(parent, i), level + 1);
            }
        }
    }
}
